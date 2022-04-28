
using System.Text;
using Monkey.Lexing;

namespace Monkey.AST;

public class BlockStatement : Statement
{
    public Token Token { get; init; } = default!;
    public List<Statement> Statements { get; } = new();

    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new();
        Statements.ForEach(s => builder.Append(s.ToString() + ";"));
        return $"{{ {builder} }}";
    }
}