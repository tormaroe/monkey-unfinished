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

# Function object type

```csharp

using Monkey.AST;

namespace Monkey.Evaluation;

public class Function : IObject
{
    public ObjectType ObjectType => ObjectType.FUNCTION;
    public List<Identifier> Parameters { get; init; } = default!;
    public BlockStatement? Body { get; init; } = default!;
    public Environment Env { get; init; } = default!;


    public string Inspect()
    {
        var paramsString = string.Join(", ", Parameters.Select(p => p.ToString()));
        return $"fn({paramsString}) {Body}"; // TODO: Newlines
    }
}

```