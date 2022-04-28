
namespace Monkey.Evaluation;

public class Error : IObject
{
    public ObjectType ObjectType => ObjectType.ERROR;
    public string Message { get; init; } = default!;

    public string Inspect()
    {
        return $"ERROR: {Message}";
    }
}
