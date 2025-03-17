using Core.Core.Visitor;

namespace Core.Core
{
    public abstract class Statement
    {
        public abstract T Accept<T>(IStatementVisitor<T> visitor);
    }

    public class PrintStatement : Statement
    {
        public readonly Expression Expression;

        public PrintStatement(Expression expression)
        {
            this.Expression = expression;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitPrintStatement(this);
    }

    public class ExpressionStatement : Statement
    {
        public readonly Expression Expression;

        public ExpressionStatement(Expression expression)
        {
            this.Expression = expression;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitExpressionStatement(this);
    }
    public class VarStatement : Statement
    {
        public VarStatement(Token name, Expression initializer)
        {
            this.Name = name;
            this.Initializer = initializer;
        }

        public readonly Token Name;
        public readonly Expression Initializer;
        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitVarStatement(this);
    }
    public class BlockStatement : Statement
    {
        public BlockStatement(List<Statement> statements)
        {
            this.Statements = statements;
        }

        public readonly List<Statement> Statements;
        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitBlockStatement(this);
    }
    public class IfStatement : Statement
    {
        public IfStatement(Expression condition, Statement thenBranch, Statement elseBranch)
        {
            this.Condition = condition;
            this.ThenBranch = thenBranch;
            this.ElseBranch = elseBranch;
        }

        public readonly Expression Condition;
        public readonly Statement ThenBranch;
        public readonly Statement ElseBranch;
        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitIfStatement(this);
    }
    public class WhileStatement : Statement
    {
        public WhileStatement(Expression condition, Statement body)
        {
            this.Condition = condition;
            this.Body = body;
        }

        public readonly Expression Condition;
        public readonly Statement Body;
        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitWhileStatement(this);
    }
    public class FunctionStatement : Statement
    {
        public FunctionStatement(Token name, List<Token> @params, List<Statement> body)
        {
            this.Name = name;
            this.@Params = @params;
            this.Body = body;
        }

        public readonly Token Name;
        public readonly List<Token> @Params;
        public readonly List<Statement> Body;
        public override T Accept<T>(IStatementVisitor<T> visitor) => visitor.VisitFunctionStatement(this);
    }
}