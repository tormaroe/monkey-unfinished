
using Monkey.Lexing;

namespace Monkey.AST;

public class BooleanLiteral : Expression
{
    public Token Token { get; init; } = default!;
    public bool Value { get; init; } = default!;

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