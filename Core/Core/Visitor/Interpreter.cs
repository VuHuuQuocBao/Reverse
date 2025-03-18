using Compiler.Enums;
using Core.Class;
using Core.Function;

namespace Core.Core.Visitor
{
    public class Interpreter : IExpressionVisitor<object>, IStatementVisitor<object>
    {
        private Environment environment = new Environment();
        public object InterpretExpresion(Expression exp) => Evaluate(exp);

        public void InterpretStatement(List<Statement> listStatement)
        {
            foreach (var statement in listStatement)
                Execute(statement);
        }

        #region Visitor function

        #region Visit Statement
        public object VisitClassStatement(Class stmt)
        {
            environment.Define(stmt.Name._lexeme, null);

            Dictionary<string, ReverseCallable> methods = new Dictionary<string, ReverseCallable>();
            foreach (FunctionStatement method in stmt.Methods)
            {
                ReverseCallable function = new ReverseCallable(method, environment);
                methods[method.Name._lexeme] = function;
            }

            ReverseClass klass = new ReverseClass(stmt.Name._lexeme, methods);
            environment.Assign(stmt.Name, klass);
            return null;
        }


        public object VisitReturnStatement(ReturnStatement stmt)
        {
            object value = null;
            if (stmt.Value != null)
                value = Evaluate(stmt.Value);

            throw new ReturnException(value);
        }


        public object VisitFunctionStatement(FunctionStatement stmt)
        {
            ReverseCallable function = new ReverseCallable(stmt, environment);
            environment.Define(stmt.Name._lexeme, function);
            return null;
        }

        public object VisitWhileStatement(WhileStatement statement)
        {
            while (IsTruthy(Evaluate(statement.Condition)))
                Execute(statement.Body);

            return null;
        }

        public object VisitIfStatement(IfStatement statement)
        {
            if (IsTruthy(Evaluate(statement.Condition)))
                Execute(statement.ThenBranch);
            else if (statement.ElseBranch != null)
                Execute(statement.ElseBranch);

            return null;
        }

        public object VisitBlockStatement(BlockStatement stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(environment));
            return null;
        }

        public object VisitVarStatement(VarStatement stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
                value = Evaluate(stmt.Initializer);

            environment.Define(stmt.Name._lexeme, value);
            return null;
        }
        public object VisitPrintStatement(PrintStatement stmt)
        {
            var value = Evaluate(stmt.Expression);
            Console.WriteLine(value);
            return null;
        }

        public object VisitExpressionStatement(ExpressionStatement stmt)
        {
            var value = Evaluate(stmt.Expression);
            return null;
        }

        #endregion

        #region Visit Expression
        public object VisitSetExpression(Set expr)
        {
            object obj = Evaluate(expr.Object);
            if (!(obj is ReverseInstance))
            {
                throw new Exception("Only instances have fields.");
            }
            object value = Evaluate(expr.Value);
            ((ReverseInstance)obj).set(expr.Name, value);
            return value;
        }

        public object VisitGetExpression(Get expr)
        {
            object obj = Evaluate(expr.Object);
            if (obj is ReverseInstance)
                return ((ReverseInstance)obj).Get(expr.Name);

            throw new Exception("Only instances have properties.");
        }

        public object VisitCallExpression(Call expr)
        {
            object callee = Evaluate(expr.Callee);
            List<object> arguments = new List<object>();
            foreach (var argument in expr.Arguments)
                arguments.Add(Evaluate(argument));

            if (callee is not IReverseCallable)
                throw new Exception(
                "Can only call functions and classes.");

            var function = (IReverseCallable)callee;

            if (arguments.Count != function.Arity())
                throw new Exception("Arguments count not match");

            return function.Call(this, arguments);
        }


        public object VisitLogicalExpression(Logical expression)
        {
            object left = Evaluate(expression.Left);

            if (expression.@Operator._type is TokenType.OR)
            {
                if (IsTruthy(left)) return left;
            }
            else
                if (!IsTruthy(left)) return left;

            return Evaluate(expression.Right);
        }

        public object VisitAssignExpr(Assign exp)
        {
            Object value = Evaluate(exp.Value);
            environment.Assign(exp.Name, value);
            return value;
        }
        public object VisitVariableExp(Variable exp) => environment.Get(exp.Name);
        public object VisitBinaryExp(Binary exp)
        {
            object left = Evaluate(exp.Left);
            object right = Evaluate(exp.Right);

            var value = exp.@Operator._type switch
            {
                TokenType.MINUS => (object)((double)left - (double)right),
                TokenType.SLASH => (double)left / (double)right,
                TokenType.STAR => (double)left * (double)right,
                TokenType.PLUS => HandleVisitBinaryExpPlus(left, right),
                TokenType.GREATER => (double)left > (double)right,
                TokenType.GREATER_EQUAL => (double)left >= (double)right,
                TokenType.LESS => (double)left < (double)right,
                TokenType.LESS_EQUAL => (double)left <= (double)right,
                TokenType.EQUAL_EQUAL => IsEqual(left, right),
                TokenType.BANG_EQUAL => !IsEqual(left, right),
                _ => null // Unreachable.
            };

            return value;
        }
        public object VisitGroupingExp(Grouping exp) => Evaluate(exp.Expression);
        public object VisitLiteralExp(Literal exp) => exp.Value;
        public object VisitUnaryExp(Unary exp)
        {
            object right = Evaluate(exp.Right);

            var value = exp.@Operator._type switch
            {
                TokenType.MINUS => (object)-(double)right,
                TokenType.BANG => !IsTruthy(right),
                _ => null
            };

            return value;
        }

        #endregion

        #endregion

        #region Helper method
        public void ExecuteBlock(List<Statement> statements, Environment environment)
        {
            Environment previous = this.environment;

            this.environment = environment;
            try
            {
                foreach (Statement statement in statements)
                    Execute(statement);
            }
            finally
            {
                this.environment = previous;
            }
        }
        private Object Evaluate(Expression expr) => expr.Accept(this);
        private void Execute(Statement stmt) => stmt.Accept(this);
        private bool IsTruthy(object obj)
        {
            if (obj is null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }
        private bool IsEqual(object a, object b)
        {
            if (a is null && b is null) return true;
            if (a is null) return false;
            return a.Equals(b);
        }

        #endregion

        #region Handle special case

        private object HandleVisitBinaryExpPlus(object left, object right)
            => (left, right) switch
            {
                (double l, double r) => (object)(l + r),
                (string l, string r) => l + r
            };

        #endregion
    }
}
