
namespace Monkey.Evaluation;

public class Null : IObject
{
    public ObjectType ObjectType => ObjectType.NULL;

    public string Inspect()
    {
        return "null";
    }
}
