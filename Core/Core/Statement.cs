using Core.Core.Visitor;

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
    public class VarStatement : Statement
    {
        public VarStatement(Token name, Expression initializer)
        {
            this.name = name;
            this.initializer = initializer;
        }

        public readonly Token name;
        public readonly Expression initializer;
        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitVarStatement(this);
    }
   public class Var : Statement
    {
        public Var(Token name, Expression initializer)
        {
            this.name = name;
            this.initializer = initializer;
        }

        public readonly Token name;
        public readonly Expression initializer;
public override T Accept<T>(IStatementVisitor<T> visitor) => throw new NotImplementedException();
    }
}