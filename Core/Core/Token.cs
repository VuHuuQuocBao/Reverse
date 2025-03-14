using Compiler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
