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

# Parse functions

```csharp

    private Expression? ParseFunctionLiteral()
    {
        var expr = new FunctionLiteral
        {
            Token = currentToken
        };

        if (!ExpectPeek(TokenType.LPAREN))
        {
            return null;
        }

        expr.Parameters.AddRange(ParseFunctionParameters());

        if (!ExpectPeek(TokenType.LBRACE))
        {
            return null;
        }

        expr.Body = ParseBlockStatement();

        return expr;
    }

    private List<Identifier> ParseFunctionParameters()
    {
        List<Identifier> identifiers = new();

        if (peekToken.TokenType == TokenType.RPAREN)
        {
            NextToken();
            return identifiers;
        }

        NextToken();

        identifiers.Add(ParseIdentifier());

        while (peekToken.TokenType == TokenType.COMMA)
        {
            NextToken();
            NextToken();
            identifiers.Add(ParseIdentifier());
        }

        if (!ExpectPeek(TokenType.RPAREN))
        {
            return new();
        }

        return identifiers;
    }


```

```csharp

        prefixParseFunctions.Add(TokenType.FUNCTION, ParseFunctionLiteral);

```