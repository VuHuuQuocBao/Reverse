using System.Text;

namespace Core.Core.Visitor
{
    public class AstPrinter : IExpressionVisitor<string>
    {
        public string VisitBinaryExp(Binary exp) => Parenthesize(exp.@Operator._lexeme, exp.Left, exp.Right);
        public string VisitGroupingExp(Grouping exp) => Parenthesize("group", exp.Expression);

        public string VisitLiteralExp(Literal exp)
        {
            if (exp.Value is null)
                return "nil";

            return exp.Value.ToString() ?? "nil";
        }

        public string VisitUnaryExp(Unary exp) => Parenthesize(exp.@Operator._lexeme, exp.Right);
        public string Parenthesize(string name, params Expression[] exprs)
        {
            var builder = new StringBuilder();
            builder.Append("(").Append(name);
            foreach (var expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");
            return builder.ToString();
        }
        public string Print(Expression expr) => expr.Accept(this);
        public string VisitVariableExp(Variable exp) => throw new NotImplementedException();
        public string VisitAssignExpr(Assign exp) => throw new NotImplementedException();
        public string VisitLogicalExpression(Logical exp) => throw new NotImplementedException();
        public string VisitCallExpression(Call exp) => throw new NotImplementedException();
        public string VisitGetExpression(Get exp) => throw new NotImplementedException();
        public string VisitSetExpression(Set exp) => throw new NotImplementedException();
    }
}
