
using Monkey.Lexing;

namespace Monkey.AST;

public class ReturnStatement : Statement
{
    public Token Token { get; init; } = default!;
    public Expression? ReturnValue { get; set; }

    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        return $"{TokenLiteral} {ReturnValue};";
    }
}
