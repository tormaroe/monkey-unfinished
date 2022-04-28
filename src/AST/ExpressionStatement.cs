
using Monkey.Lexing;

namespace Monkey.AST;

/*
    This type only exists so that an expression can also be a statement
    (and then be part of a program without being part of a let or return statement).
*/
public class ExpressionStatement : Statement
{
    public Token Token { get; init; } = default!;
    public Expression? Expression { get; set; } = default!;

    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        return Expression?.ToString() ?? string.Empty;
    }
}