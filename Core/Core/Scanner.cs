using Compiler.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Compiler.Core
{
    public class Scanner
    {
        private string _source;

        private List<Token> _listToken = new();
        private readonly Dictionary<string, TokenType> _systemToken = new()
                    {
                        {"and", TokenType.AND},
                        {"class", TokenType.CLASS},
                        {"else", TokenType.ELSE},
                        {"false", TokenType.FALSE},
                        {"for", TokenType.FOR},
                        {"fun", TokenType.FUN},
                        {"if", TokenType.IF},
                        {"nil", TokenType.NIL},
                        {"or", TokenType.OR},
                        {"print", TokenType.PRINT},
                        {"return", TokenType.RETURN},
                        {"super", TokenType.SUPER},
                        {"this", TokenType.THIS},
                        {"true", TokenType.TRUE},
                        {"var", TokenType.VAR},
                        {"while", TokenType.WHILE}
                    };

        private int _start = 0;
        private int _current = 0;
        private int _line = 1;
        public List<Token> ScanTokens(string source)
        {
            _source = source;
            while (IsAtEnd() is false)
            {
                _start = _current;
                ScanToken();
            }

            _listToken.Add(new Token(TokenType.EOF, "", null!, _line));
            return _listToken;
        }

        public void ScanToken()
        {
            char c = Advance();
            (c switch // TODO: add default case => error
            {
                '(' => (Action)(() => AddToken(TokenType.LEFT_PAREN)),
                ')' => () => AddToken(TokenType.RIGHT_PAREN),
                '{' => () => AddToken(TokenType.LEFT_BRACE),
                '}' => () => AddToken(TokenType.RIGHT_BRACE),
                ',' => () => AddToken(TokenType.COMMA),
                '.' => () => AddToken(TokenType.DOT),
                '-' => () => AddToken(TokenType.MINUS),
                '+' => () => AddToken(TokenType.PLUS),
                ';' => () => AddToken(TokenType.SEMICOLON),
                '*' => () => AddToken(TokenType.STAR),
                '/' => () => CheckEdgeCase(TokenType.SLASH),
                '!' => () => CheckEdgeCase(TokenType.BANG),
                '=' => () => CheckEdgeCase(TokenType.EQUAL),
                '>' => () => CheckEdgeCase(TokenType.GREATER),
                '<' => () => CheckEdgeCase(TokenType.LESS),
                ' ' or '\r' or '\t' => () => DoNothing(),
                '\n' => () => NewLine(),

                // literals
                '"' => () => AddString(),

                // numbers ...
                _ => () => CheckDefault(c)
            })();
        }
        private bool IsAtEnd() => _current >= _source.Length;
        private char Advance()
        {
            if (_current < _source.Length)
            {
                _current++;
                return _source[_current - 1];
            }
            return '\0'; // TODO: think something else
        }
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }
        private void AddToken(TokenType type) => AddToken(type, null!);
        private void AddToken(TokenType type, object literal)
        {
            var text = _source.Substring(_start, _current - _start);
            _listToken.Add(new Token(type, text, literal, _line));
        }
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            _current++;
            return true;
        }
        private void CheckEdgeCase(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.BANG:
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case TokenType.EQUAL:
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case TokenType.GREATER:
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case TokenType.LESS:
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS_EQUAL);
                    break;
                case TokenType.SLASH:
                    if (Match('/'))
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    else
                        AddToken(TokenType.SLASH);
                    break;
                default: break;
            }
        }
        private void DoNothing()
        {
            return;
        }

        private void NewLine() => _line++;

        private void AddString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() is '\n') _line++;
                Advance();
            }
            Advance();

            var value = _source.Substring(_start + 1, _current - _start - 2);
            AddToken(TokenType.STRING, value);
        }
        private void AddNumber()
        {
            while (IsDigit(Peek())) Advance();

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // consume . character
                Advance();

                while (IsDigit(Peek())) Advance();
            }

            AddToken(TokenType.NUMBER, double.Parse(_source.Substring(_start, _current - _start)));
        }
        private void CheckDefault(char c)
        {
            if (IsDigit(c))
                AddNumber();
            else if (IsAlpha(c))
                AddIdentifier();
            //
        }

        private void AddIdentifier()
        {
            while (isAlphaNumeric(Peek())) Advance();

            var text = _source.Substring(_start, _current - _start);

            /*            _systemToken.TryGetValue(text, out var type);
                        if(type ) type = TokenType.IDENTIFIER;*/

            var type = _systemToken.ContainsKey(text) switch
            {
                false => TokenType.IDENTIFIER,
                _ => _systemToken[text],
            };

            AddToken(type);
        }
        private bool IsDigit(char c) => c >= '0' && c <= '9';
        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }
        private bool IsAlpha(char c) => (c is >= 'a' and <= 'z') || (c is >= 'A' and <= 'Z') || c is '_';
        private bool isAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

    }
}