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

# Parse function call

```csharp

    private Expression ParseCallFunction(Expression function)
    {
        return new CallExpression
        {
            Token = currentToken,
            Function = function,
            Arguments = ParseCallArguments()
        };
    }

    private List<Expression> ParseCallArguments()
    {
        List<Expression> args = new();

        if (peekToken.TokenType == TokenType.RPAREN)
        {
            NextToken();
            return args;
        }

        NextToken();

        args.Add(ParseExpression(ExpressionPrecedence.LOWEST)!);

        while (peekToken.TokenType == TokenType.COMMA)
        {
            NextToken();
            NextToken();
            args.Add(ParseExpression(ExpressionPrecedence.LOWEST)!);
        }

        if (!ExpectPeek(TokenType.RPAREN))
        {
            return new();
        }

        return args;
    }


```

```csharp

        infixParseFunctions.Add(TokenType.LPAREN, ParseCallFunction);

```