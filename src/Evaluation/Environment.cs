
namespace Monkey.Evaluation;

public class Environment
{
    private Dictionary<string, IObject> store = new();
    private Environment? outerEnv;

    public bool TryGet(String name, out IObject? o)
    {
        bool ok = store.TryGetValue(name, out o);

        if (!ok && outerEnv != null)
        {
            ok = outerEnv.TryGet(name, out o);
        }

        return ok;
    }

    public IObject Set(string name, IObject o)
    {
        store[name] = o;
        return o;
    }

    public Environment NewEnclosed()
    {
        var env = new Environment();
        env.outerEnv = this;
        return env;
    }
}