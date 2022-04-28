
using System.Text;

namespace Monkey.AST;

public class Program : Node
{
    public List<Statement> Statements { get; } = new();

    public string TokenLiteral
    {
        get
        {
            if (Statements?.Count > 0)
            {
                return Statements.First().TokenLiteral;
            }
            return string.Empty;
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new();
        Statements.ForEach(s => builder.Append(s.ToString()));
        return builder.ToString();
    }
}
