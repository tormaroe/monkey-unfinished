
namespace Monkey.Lexing;

public static class Keywords
{
    private static Dictionary<string, TokenType> map = new()
    {
        { "let", TokenType.LET },
        { "true", TokenType.TRUE },
        { "false", TokenType.FALSE },
        { "if", TokenType.IF },
        { "else", TokenType.ELSE },
    };

    public static TokenType LookupIdent(string ident)
    {
        if (map.TryGetValue(ident, out TokenType tok))
        {
            return tok;
        }
        return TokenType.IDENT;
    }
}