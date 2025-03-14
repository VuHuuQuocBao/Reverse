using Core.Core;
using Core.Core.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core
{
    public abstract class Statement
    {
        public abstract T Accept<T>(IStatementVisitor<T> visitor);
    }

    public class PrintStatement : Statement
    {
        public readonly Expression expression;

        public PrintStatement(Expression expression)
        {
            this.expression = expression;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitPrintStatement(this);
    }

    public class ExpressionStatement : Statement
    {
        public readonly Expression expression;

        public ExpressionStatement(Expression expression)
        {
            this.expression = expression;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitExpressionStatement(this);
    }
}
