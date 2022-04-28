namespace Monkey.Evaluation;

public class ReturnValue : IObject
{
    public ObjectType ObjectType => ObjectType.RETURN_VALUE;
    public IObject Value { get; init; } = default!;
    public string Inspect()
    {
        return Value.Inspect();
    }
}
