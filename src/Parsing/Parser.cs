
using Monkey.AST;
using Monkey.Lexing;

namespace Monkey.Parsing;

public class Parser
{
    private Token currentToken;
    private Token peekToken;
    private readonly Lexer lexer;

    private Dictionary<TokenType, Func<Expression?>> prefixParseFunctions;
    private Dictionary<TokenType, Func<Expression, Expression>> infixParseFunctions;

    private Dictionary<TokenType, ExpressionPrecedence> precedences = new Dictionary<TokenType, ExpressionPrecedence>
    {
        { TokenType.EQ, ExpressionPrecedence.EQUALS },
        { TokenType.NOT_EQ, ExpressionPrecedence.EQUALS },
        { TokenType.LT, ExpressionPrecedence.LESSGREATER },
        { TokenType.GT, ExpressionPrecedence.LESSGREATER },
        { TokenType.PLUS, ExpressionPrecedence.SUM },
        { TokenType.MINUS, ExpressionPrecedence.SUM },
        { TokenType.SLASH, ExpressionPrecedence.PRODUCT },
        { TokenType.ASTERISK, ExpressionPrecedence.PRODUCT },
        { TokenType.LPAREN, ExpressionPrecedence.CALL },
    };

    public List<string> Errors { get; } = new();

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;

        prefixParseFunctions = new();
        infixParseFunctions = new();

        prefixParseFunctions.Add(TokenType.IDENT, ParseIdentifier);
        prefixParseFunctions.Add(TokenType.INT, ParseIntegerLiteral);
        prefixParseFunctions.Add(TokenType.BANG, ParsePrefixExpression);
        prefixParseFunctions.Add(TokenType.MINUS, ParsePrefixExpression);
        prefixParseFunctions.Add(TokenType.TRUE, ParseBoolean);
        prefixParseFunctions.Add(TokenType.FALSE, ParseBoolean);
        prefixParseFunctions.Add(TokenType.LPAREN, ParseGroupedExpression);
        prefixParseFunctions.Add(TokenType.IF, ParseIfExpression);

        infixParseFunctions.Add(TokenType.PLUS, ParseInfixExpression);
        infixParseFunctions.Add(TokenType.MINUS, ParseInfixExpression);
        infixParseFunctions.Add(TokenType.SLASH, ParseInfixExpression);
        infixParseFunctions.Add(TokenType.ASTERISK, ParseInfixExpression);
        infixParseFunctions.Add(TokenType.EQ, ParseInfixExpression);
        infixParseFunctions.Add(TokenType.NOT_EQ, ParseInfixExpression);
        infixParseFunctions.Add(TokenType.LT, ParseInfixExpression);
        infixParseFunctions.Add(TokenType.GT, ParseInfixExpression);

        /* Convincing the compiler that current and peek Tokens will never be null */
        currentToken = new Token(TokenType.LBRACE, "{");
        peekToken = new Token(TokenType.RBRACE, "}");

        NextToken();
        NextToken();
    }

    public AST.Program? ParseProgram()
    {
        AST.Program program = new();

        while (currentToken.TokenType != TokenType.EOF)
        {
            AST.Statement? stmt = ParseStatement();

            if (stmt != null)
            {
                program.Statements.Add(stmt);
            }

            NextToken();
        }

        return program;
    }

    private Statement? ParseStatement()
    {
        switch (currentToken.TokenType)
        {
            case TokenType.LET:
                return ParseLetStatement();
            case TokenType.RETURN:
                return ParseReturnStatement();
            default:
                return ParseExpressionStatement();
        }
    }

    private Expression? ParseGroupedExpression()
    {
        NextToken();
        var expr = ParseExpression(ExpressionPrecedence.LOWEST);

        if (!ExpectPeek(TokenType.RPAREN))
        {
            return null;
        }

        return expr;
    }

    private ExpressionStatement ParseExpressionStatement()
    {
        var stmt = new ExpressionStatement{ Token = currentToken };

        stmt.Expression = ParseExpression(ExpressionPrecedence.LOWEST);

        if (peekToken.TokenType == TokenType.SEMICOLON)
        {
            // Yes, semicolons for expression statements are optional!
            NextToken();
        }

        return stmt;
    }

    private Expression? ParseExpression(ExpressionPrecedence precedence)
    {
        if (prefixParseFunctions.TryGetValue(currentToken.TokenType, out Func<Expression?>? parser))
        {
            var leftExpr = parser.Invoke();

            while (peekToken.TokenType != TokenType.SEMICOLON && precedence < PeekPrecedence())
            {
                bool infixFound = infixParseFunctions.TryGetValue(peekToken.TokenType, out Func<Expression, Expression>? infix);

                if (!infixFound)
                {
                    return leftExpr;
                }

                NextToken();

                if (leftExpr == null)
                    throw new Exception("Implementation error 1");

                leftExpr = infix!.Invoke(leftExpr);
            }

            return leftExpr;
        }
        
        NoPrefixParseFunction(currentToken.TokenType);
        return null;
    }

    private Expression ParsePrefixExpression()
    {
        var expr = new PrefixExpression
        {
            Token = currentToken,
            Operator = currentToken.Literal,
        };

        NextToken();
        expr.Right = ParseExpression(ExpressionPrecedence.PREFIX);
        return expr;
    }

    private Expression ParseInfixExpression(Expression left)
    {
        var expr = new InfixExpression
        {
            Token = currentToken,
            Operator = currentToken.Literal,
            Left = left,
        };

        var precedence = CurrentPrecedence();
        NextToken();
        expr.Right = ParseExpression(precedence);
        return expr;
    }

    private Identifier ParseIdentifier()
    {
        return new Identifier
        {
            Token = currentToken,
            Value = currentToken.Literal,
        };
    }

    private Expression? ParseIntegerLiteral()
    {
        if (Int64.TryParse(currentToken.Literal, out long result))
        {
            return new IntegerLiteral
            {
                Token = currentToken,
                Value = result,
            };
        }
        else
        {
            Errors.Add($"could not parse {currentToken.Literal} as integer");
            return null;
        }
    }

    private Expression ParseBoolean()
    {
        return new BooleanLiteral
        {
            Token = currentToken,
            Value = currentToken.TokenType == TokenType.TRUE,
        };
    }

    private Expression? ParseIfExpression()
    {
        var expr = new IfExpression
        {
            Token = currentToken
        };

        if (!ExpectPeek(TokenType.LPAREN))
        {
            return null;
        }

        NextToken();

        expr.Condition = ParseExpression(ExpressionPrecedence.LOWEST);

        if (!ExpectPeek(TokenType.RPAREN))
        {
            return null;
        }

        if (!ExpectPeek(TokenType.LBRACE))
        {
            return null;
        }

        expr.Consequence = ParseBlockStatement();

        if (peekToken.TokenType == TokenType.ELSE)
        {
            NextToken();

            if (!ExpectPeek(TokenType.LBRACE))
            {
                return null;
            }

            expr.Alternative = ParseBlockStatement();
        }

        return expr;
    }


    private BlockStatement? ParseBlockStatement()
    {
        var block = new BlockStatement
        {
            Token = currentToken
        };

        NextToken();

        while (currentToken.TokenType != TokenType.RBRACE && currentToken.TokenType != TokenType.EOF)
        {
            var stmt = ParseStatement();

            if (stmt != null)
            {
                block.Statements.Add(stmt);
            }

            NextToken();
        }

        return block;
    }

    private ReturnStatement ParseReturnStatement()
    {
        var stmt = new ReturnStatement{ Token = currentToken };

        NextToken();

        stmt.ReturnValue = ParseExpression(ExpressionPrecedence.LOWEST);

        if (currentToken.TokenType == TokenType.SEMICOLON) 
        {
            NextToken();
        }

        return stmt;
    }

    private LetStatement? ParseLetStatement()
    {
        var stmt = new LetStatement{ Token = currentToken };

        if (!ExpectPeek(TokenType.IDENT))
        {
            return null;
        }

        stmt.Name = new(){ Token = currentToken, Value = currentToken.Literal };

        if (!ExpectPeek(TokenType.ASSIGN))
        {
            return null;
        }

        NextToken();

        stmt.Value = ParseExpression(ExpressionPrecedence.LOWEST);

        if (currentToken.TokenType == TokenType.SEMICOLON)
        {
            NextToken();
        }
        
        return stmt;
    }

    private bool ExpectPeek(TokenType type)
    {
        if (peekToken.TokenType == type)
        {
            NextToken();
            return true;
        }
        PeekError(type);
        return false;
    }

    private void PeekError(TokenType type)
    {
        Errors.Add($"expected next token to be {type}, got {peekToken.TokenType} instead");
    }

    private void NoPrefixParseFunction(TokenType type)
    {
        Errors.Add($"no prefix parse function for {type} found");   
    }

    private void NextToken()
    {
        currentToken = peekToken;
        peekToken = lexer.NextToken();
    }

    private ExpressionPrecedence PeekPrecedence()
    {
        if (precedences.TryGetValue(peekToken.TokenType, out ExpressionPrecedence value))
        {
            return value;
        }
        return ExpressionPrecedence.LOWEST;
    }

    private ExpressionPrecedence CurrentPrecedence()
    {
        if (precedences.TryGetValue(currentToken.TokenType, out ExpressionPrecedence value))
        {
            return value;
        }
        return ExpressionPrecedence.LOWEST;
    }
}