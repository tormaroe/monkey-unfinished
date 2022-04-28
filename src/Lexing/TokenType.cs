
namespace Monkey.Lexing;

public enum TokenType
{
    ILLEGAL,
    EOF,
    
    IDENT, // add, foobar, x, y, ...
    INT, // 123456
    
    ASSIGN, // =
    PLUS, // +
    MINUS, // -
    BANG, // !
    ASTERISK, // *
    SLASH, // /

    LT, // <
    GT, // >
    EQ, // ==
    NOT_EQ, // !=
    
    COMMA,
    SEMICOLON,
    
    LPAREN,
    RPAREN,
    LBRACE,
    RBRACE,

    LET,
    TRUE,
    FALSE,
    IF,
    ELSE,
    RETURN
}
