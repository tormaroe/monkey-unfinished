
namespace Monkey.Evaluation;

public interface IObject
{
    public ObjectType ObjectType { get; }
    string Inspect();
}
