using Compiler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core.Visitor
{
    public class AstPrinter : IVisitor<string>
    {
        public string VisitBinaryExp(Binary exp) => Parenthesize(exp.@operator._lexeme, exp.left, exp.right);
        public string VisitGroupingExp(Grouping exp) => Parenthesize("group", exp.expression);

        public string VisitLiteralExp(Literal exp)
        {
            if (exp.value is null)
                return "nil";

            return exp.value.ToString() ?? "nil";
        }

        public string VisitUnaryExp(Unary exp) => Parenthesize(exp.@operator._lexeme, exp.right);
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
    }
}
