
using Monkey.Lexing;

namespace Monkey.AST;

public class LetStatement : Statement
{
    public Token Token { get; init; } = default!;
    public Identifier Name { get; set; } = default!;
    public Expression? Value { get; set; }

    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        return $"{TokenLiteral} {Name} = {Value};";
    }
}
