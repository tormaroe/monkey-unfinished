
namespace Monkey.Evaluation;

public class Boolean : IObject
{
    public bool Value { get; set; }
    public ObjectType ObjectType => ObjectType.BOOLEAN;

    public string Inspect()
    {
        return Value.ToString();
    }
}
