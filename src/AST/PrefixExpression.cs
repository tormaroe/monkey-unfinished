
using Monkey.Lexing;

namespace Monkey.AST;

public class PrefixExpression : Expression
{
    public Token Token { get; init; } = default!;
    public string Operator { get; init; } = default!;
    public Expression? Right { get; set; }
    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        return $"({Operator}{Right})";
    }
}
