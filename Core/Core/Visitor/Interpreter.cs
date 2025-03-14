﻿using Compiler.Enums;

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
        public object VisitBlockStatement(BlockStatement stmt)
        {
            ExecuteBlock(stmt.statements, new Environment(environment));
            return null;
        }

        public object VisitVarStatement(VarStatement stmt)
        {
            object value = null;
            if (stmt.initializer != null)
                value = Evaluate(stmt.initializer);

            environment.Define(stmt.name._lexeme, value);
            return null;
        }
        public object VisitPrintStatement(PrintStatement stmt)
        {
            var value = Evaluate(stmt.expression);
            Console.WriteLine(value);
            return null;
        }

        public object VisitExpressionStatement(ExpressionStatement stmt)
        {
            var value = Evaluate(stmt.expression);
            Console.WriteLine(value);
            return null;
        }

        #endregion

        #region Visit Expression
        public object VisitAssignExpr(Assign exp)
        {
            Object value = Evaluate(exp.value);
            environment.Assign(exp.name, value);
            return value;
        }
        public object VisitVariableExp(Variable exp) => environment.Get(exp.name);
        public object VisitBinaryExp(Binary exp)
        {
            object left = Evaluate(exp.left);
            object right = Evaluate(exp.right);

            var value = exp.@operator._type switch
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
        public object VisitGroupingExp(Grouping exp) => Evaluate(exp.expression);
        public object VisitLiteralExp(Literal exp) => exp.value;
        public object VisitUnaryExp(Unary exp)
        {
            object right = Evaluate(exp.right);

            var value = exp.@operator._type switch
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
        private void ExecuteBlock(List<Statement> statements, Environment environment)
        {
            Environment previous = this.environment;

            this.environment = environment;
            foreach (Statement statement in statements)
                Execute(statement);
            this.environment = previous;
        }
        private Object Evaluate(Expression expr) => expr.Accept(this);
        private void Execute(Statement stmt) => stmt.Accept(this);
        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            return true;
        }
        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
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
