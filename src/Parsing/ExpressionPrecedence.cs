namespace Monkey.Parsing;

public enum ExpressionPrecedence
{
    LOWEST,
    EQUALS, // ==
    LESSGREATER, // > or <
    SUM, // +
    PRODUCT, // *
    PREFIX, // -X or !X
    CALL, // myFunction(X)
}
