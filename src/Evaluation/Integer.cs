
namespace Monkey.Evaluation;

public class Integer : IObject
{
    public Int64 Value { get; set; }
    public ObjectType ObjectType => ObjectType.INTEGER;

    public string Inspect()
    {
        return Value.ToString();
    }
}
