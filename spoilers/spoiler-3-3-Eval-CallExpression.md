```



========================================================================================




 ::::::::  :::::::::   :::::::: ::::::::::: :::        :::::::::: :::::::::   ::::::::  
:+:    :+: :+:    :+: :+:    :+:    :+:     :+:        :+:        :+:    :+: :+:    :+: 
+:+        +:+    +:+ +:+    +:+    +:+     +:+        +:+        +:+    +:+ +:+        
+#++:++#++ +#++:++#+  +#+    +:+    +#+     +#+        +#++:++#   +#++:++#:  +#++:++#++ 
       +#+ +#+        +#+    +#+    +#+     +#+        +#+        +#+    +#+        +#+ 
#+#    #+# #+#        #+#    #+#    #+#     #+#        #+#        #+#    #+# #+#    #+# 
 ########  ###         ######## ########### ########## ########## ###    ###  ########  




========================================================================================



```

# Evaluate CallExpression

```csharp

            case AST.CallExpression ce:
                var function = Eval(ce.Function, env);
                if (IsError(function))
                {
                    return function;
                }
                var args = EvalExpressions(ce.Arguments, env);
                if (args.Count == 1 && IsError(args[0]))
                {
                    return args[0];
                }
                return ApplyFunction(function, args);

```

```csharp
    private List<IObject> EvalExpressions(List<Expression> expressions, Environment env)
    {
        List<IObject> result = new();

        foreach(var expr in expressions)
        {
            var evaluated = Eval(expr, env);
            if (IsError(evaluated))
            {
                return new(){ evaluated };
            }
            result.Add(evaluated);
        }

        return result;
    }

    private IObject ApplyFunction(IObject function, List<IObject> args)
    {
        if (function is Function)
        {
            var functionTyped = (Function)function;
            var extendedEnv = ExtendedFunctionEnv(functionTyped, args);
            var evaluated = Eval(functionTyped.Body, extendedEnv);
            return UnwrapReturnValue(evaluated);
        }
        else
        {
            return NewError($"not a function: {function.ObjectType}");
        }
    }

    private Environment ExtendedFunctionEnv(Function function, List<IObject> args)
    {
        var env = function.Env.NewEnclosed();
        for(int i=0; i<args.Count; i++)
        {
            env.Set(
                name: function.Parameters[i].Value,
                o: args[i]
            );
        }
        return env;
    }

    private IObject UnwrapReturnValue(IObject o)
    {
        if (o is ReturnValue)
        {
            return ((ReturnValue)o).Value;
        }
        return o;
    }

```