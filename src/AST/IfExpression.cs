using Monkey.Lexing;

namespace Monkey.AST;

public class IfExpression : Expression
{
    public Token Token { get; init; } = default!;
    public Expression? Condition { get; set; } = default!;
    public BlockStatement? Consequence { get; set; } = default!;
    public BlockStatement? Alternative { get; set; } = default!;

    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        var result = $"if ({Condition}) {Consequence}";
        if (Alternative != null)
        {
            result += $" else {Alternative}";
        }
        return result;
    }
}
