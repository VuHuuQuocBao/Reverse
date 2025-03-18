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

        private Statement ClassDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expect class name.");
            Consume(TokenType.LEFT_BRACE, "Expect '{' before class body.");
            List<FunctionStatement> methods = new List<FunctionStatement>();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                methods.Add(Function("method"));
            }

            Consume(TokenType.RIGHT_BRACE, "Expect '}' after class body.");
            return new Class(name, methods);
        }

        private Statement ReturnStatement()
        {
            Token keyword = Previous();
            Expression value = null;

            if (!Check(TokenType.SEMICOLON))
                value = Expression();

            Consume(TokenType.SEMICOLON, "Expect ';' after return value.");
            return new ReturnStatement(keyword, value);
        }

        private FunctionStatement Function(string kind)
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expect " + kind + " name.");
            Consume(TokenType.LEFT_PAREN, "Expect '(' after " + kind + " name.");
            List<Token> parameters = new();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    if (parameters.Count >= 255)
                        throw new Exception("Can't have more than 255 parameters.");
                    parameters.Add(
                    Consume(TokenType.IDENTIFIER, "Expect parameter name."));
                }
                while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after parameters.");

            Consume(TokenType.LEFT_BRACE, "Expect '{' before " + kind + " body.");
            List<Statement> body = Block();
            return new FunctionStatement(name, parameters, body);
        }
        private Statement WhileStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'while'.");
            Expression condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after condition.");
            Statement body = Statement();
            return new WhileStatement(condition, body);
        }

        private Statement IfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
            Expression condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after if condition.");

            Statement thenBranch = Statement();
            Statement elseBranch = null;

            if (Match(TokenType.ELSE))
                elseBranch = Statement();

            return new IfStatement(condition, thenBranch, elseBranch);
        }

        private Statement Declaration()
        {
            if (Match(TokenType.CLASS)) return ClassDeclaration();
            if (Match(TokenType.FUN)) return Function("function");
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
            if (Match(TokenType.RETURN)) return ReturnStatement();

            if (Match(TokenType.WHILE)) return WhileStatement();

            if (Match(TokenType.IF)) return IfStatement();

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
        private Expression Call()
        {
            Expression expr = Primary();
            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                    expr = FinishCall(expr);
                else if (Match(TokenType.DOT))
                {
                    Token name = Consume(TokenType.IDENTIFIER,
                    "Expect property name after '.'.");
                    expr = new Get(expr, name);
                }
                else
                    break;
            }

            return expr;
        }
        private Expression FinishCall(Expression callee)
        {
            List<Expression> arguments = new List<Expression>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(Expression());
                }
                while (Match(TokenType.COMMA));
            }

            Token paren = Consume(TokenType.RIGHT_PAREN, "Expect ')' after arguments.");
            return new Call(callee, paren, arguments);
        }
        private Expression And()
        {
            Expression expression = Equality();
            while (Match(TokenType.AND))
            {
                Token operatorToken = Previous();
                Expression right = Equality();
                expression = new Logical(expression, operatorToken, right);
            }
            return expression;
        }
        private Expression Or()
        {
            Expression expression = And();
            while (Match(TokenType.OR))
            {
                Token operatorToken = Previous();
                Expression right = And();
                expression = new Logical(expression, operatorToken, right);
            }
            return expression;
        }
        private Expression Assignment()
        {
            Expression expr = Or();

            //Expression expr = Equality(); // change after adding if else statement
            if (Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                Expression value = Assignment();
                if (expr is Variable)
                {
                    Token name = ((Variable)expr).Name;
                    return new Assign(name, value);
                }
                else if (expr is Get)
                {
                    Get get = (Get)expr;
                    return new Set(get.Object, get.Name, value);
                }

                //error(equals, "Invalid assignment target.");
            }
            return expr;
        }
        private Expression Expression() => Assignment();
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
            return Call();
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
