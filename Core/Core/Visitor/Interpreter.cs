using Compiler.Core;
using Compiler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core.Visitor
{
    public class Interpreter : IVisitor<object>
    {

        public object Interpret(Expression exp) => Evaluate(exp);

        #region Visitor function
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

        #region Helper method
        private Object Evaluate(Expression expr) => expr.Accept(this);
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
