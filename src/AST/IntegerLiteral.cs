
using System.Text;
using Monkey.Lexing;

namespace Monkey.AST;

public class IntegerLiteral : Expression
{
    public Token Token { get; init; } = default!;
    public Int64 Value { get; init; } = default!;

    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        return Token.Literal;
    }
}
