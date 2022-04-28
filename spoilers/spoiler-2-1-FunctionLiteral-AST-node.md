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

# FunctionLiteral AST node

```csharp

using Monkey.Lexing;

namespace Monkey.AST;

public class FunctionLiteral : Expression
{
    public Token Token { get; init; } = default!;
    public List<Identifier> Parameters { get; init; } = new();
    public BlockStatement? Body { get; set; }

    public string TokenLiteral
    {
        get
        {
            return Token.Literal;
        }
    }

    public override string ToString()
    {
        var paramsString = string.Join(", ", Parameters.Select(p => p.ToString()));
        return $"{TokenLiteral}({paramsString}) {Body}";
    }
}

```