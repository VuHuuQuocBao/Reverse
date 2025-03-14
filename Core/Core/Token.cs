using Compiler.Enums;

namespace Core.Core
{
    public class Token
    {
        public readonly TokenType _type;
        public readonly string _lexeme;
        public readonly Object _literal;
        public readonly int _line;
        public Token(TokenType type, String lexeme, object literal, int line)
        {
            this._type = type;
            this._lexeme = lexeme;
            this._literal = literal;
            this._line = line;
        }
        public string ToString() => _type + " " + _lexeme + " " + _literal;
    }
}
