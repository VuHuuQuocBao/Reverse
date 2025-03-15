using Compiler.Enums;

namespace Core.Core
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this._tokens = tokens;
        }

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek()._type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }

        private bool IsAtEnd() => Peek()._type == TokenType.EOF;

        private Token Peek() => _tokens[current];

        private Token Previous() => _tokens[current - 1];

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            //throw Error(Peek(), message);
            throw new Exception(message);
        }


        #region Visitor Function

        #region Statement

        private Statement Declaration()
        {
            if (Match(TokenType.VAR)) return VarDeclaration();
            return Statement();
        }

        private Statement VarDeclaration()
        {
            var name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

            Expression initializer = null;
            if (Match(TokenType.EQUAL))
                initializer = Expression();

            Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
            return new VarStatement(name, initializer);
        }
        private Statement Statement()
        {
            if (Match(TokenType.PRINT)) return PrintStatement();

            if (Match(TokenType.LEFT_BRACE)) return new BlockStatement(Block());

            return ExpressionStatement();
        }

        private List<Statement> Block()
        {
            List<Statement> statements = new List<Statement>();
            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }
            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            return statements;
        }

        private Statement PrintStatement()
        {
            Expression value = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new PrintStatement(value);
        }
        private Statement ExpressionStatement()
        {
            Expression expr = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new ExpressionStatement(expr);
        }

        #endregion

        #region Expression

        private Expression assignment()
        {
            Expression expr = Equality();
            if (Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                Expression value = assignment();
                if (expr is Variable) {
                    Token name = ((Variable)expr).name;
                    return new Assign(name, value);
                }
                //error(equals, "Invalid assignment target.");
            }
            return expr;
        }
        private Expression Expression() => Equality();

        private Expression Equality()
        {
            Expression exp = Comparison();
            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token @operator = Previous();
                Expression right = Comparison();
                exp = new Binary(exp, @operator, right);
            }
            return exp;
        }

        private Expression Comparison()
        {
            Expression expr = Term();
            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token @operator = Previous();
                Expression right = Term();
                expr = new Binary(expr, @operator, right);
            }
            return expr;
        }

        private Expression Term()
        {
            Expression expr = Factor();
            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token @operator = Previous();
                Expression right = Factor();
                expr = new Binary(expr, @operator, right);
            }
            return expr;
        }

        private Expression Factor()
        {
            Expression expr = Unary();
            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token @operator = Previous();
                Expression right = Unary();
                expr = new Binary(expr, @operator, right);
            }
            return expr;
        }

        private Expression Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token @operator = Previous();
                Expression right = Unary();
                return new Unary(@operator, right);
            }
            return Primary();
        }

        private Expression Primary()
        {
            if (Match(TokenType.FALSE)) return new Literal(false);
            if (Match(TokenType.TRUE)) return new Literal(true);
            if (Match(TokenType.NIL)) return new Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING))
                return new Literal(Previous()._literal);

            if (Match(TokenType.IDENTIFIER))
                return new Variable(Previous());

            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Grouping(expr);
            }

            return null; // Added a return statement to handle cases where none of the conditions are met
        }

        #endregion

        #endregion

        public Expression Parse() => Expression();

        public List<Statement> ParseStatement()
        {
            var statements = new List<Statement>();

            while (!IsAtEnd())
                statements.Add(Declaration());

            return statements;
        }
    }

}
