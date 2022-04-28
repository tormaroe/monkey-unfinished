
using Monkey.Lexing;

namespace Monkey.AST;

public class Identifier : Expression
{
    public Token Token { get; init; } = default!;
    public string Value { get; init; } = default!;

    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        return Value;
    }
}
