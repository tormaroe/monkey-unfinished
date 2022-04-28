
namespace Monkey.Lexing;

public class Lexer
{
    const char NULL_CHAR = '\0';

    private string input;
    private int position;
    private int readPosition;
    private char ch;

    public Lexer(string input)
    {
        this.input = input;
        ReadChar();
    }

    public Token NextToken()
    {
        Token tok;

        SkipWhitespace();

        switch(ch) 
        {
            case '=':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    tok = new(TokenType.EQ, "==");
                }
                else
                {
                    tok = new(TokenType.ASSIGN, ch.ToString());
                }
                break;
            case ';':
                tok = new(TokenType.SEMICOLON, ch.ToString());
                break;
            case '(':
                tok = new(TokenType.LPAREN, ch.ToString());
                break;
            case ')':
                tok = new(TokenType.RPAREN, ch.ToString());
                break;
            case ',':
                tok = new(TokenType.COMMA, ch.ToString());
                break;
            case '+':
                tok = new(TokenType.PLUS, ch.ToString());
                break;
            case '-':
                tok = new(TokenType.MINUS, ch.ToString());
                break;
            case '!':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    tok = new(TokenType.NOT_EQ, "!=");
                }
                else
                {
                    tok = new(TokenType.BANG, ch.ToString());
                }
                break;
            case '/':
                tok = new(TokenType.SLASH, ch.ToString());
                break;
            case '*':
                tok = new(TokenType.ASTERISK, ch.ToString());
                break;
            case '<':
                tok = new(TokenType.LT, ch.ToString());
                break;
            case '>':
                tok = new(TokenType.GT, ch.ToString());
                break;
            case '{':
                tok = new(TokenType.LBRACE, ch.ToString());
                break;
            case '}':
                tok = new(TokenType.RBRACE, ch.ToString());
                break;
            case NULL_CHAR:
                tok = new(TokenType.EOF, string.Empty);
                break;
            default:
                if (IsLetter(ch))
                {
                    var literal = ReadIdentifier();
                    tok = new(Keywords.LookupIdent(literal), literal);
                    return tok;
                }
                else if (char.IsDigit(ch))
                {
                    tok = new(TokenType.INT, ReadNumber());
                    return tok;
                }
                tok = new(TokenType.ILLEGAL, ch.ToString());
                break;
        }

        ReadChar();
        return tok;
    }

    private void SkipWhitespace()
    {
        while(ch is ' ' or '\t' or '\n' or '\r')
        {
            ReadChar();
        }
    }

    private string ReadIdentifier()
    {
        var start = position;
        while (IsLetter(ch))
        {
            ReadChar();
        }
        return input.Substring(start, position - start);
    }

    private string ReadNumber()
    {
        var start = position;
        while (char.IsDigit(ch))
        {
            ReadChar();
        }
        return input.Substring(start, position - start);
    }

    private void ReadChar()
    {
        if (readPosition >= input.Length)
        {
            ch = NULL_CHAR;
        }
        else
        {
            ch = input[readPosition];
        }
        position = readPosition;
        readPosition++;
    }

    private char PeekChar()
    {
        if (readPosition >= input.Length)
        {
            return NULL_CHAR;
        }
        else
        {
            return input[readPosition];
        }
    }

    private bool IsLetter(char c)
    {
        return c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '_';
    }
}