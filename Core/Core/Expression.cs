using Core.Core.Visitor;


namespace Core.Core
{
    public abstract class Expression
    {
        public abstract T Accept<T>(IExpressionVisitor<T> visitor);
    }
    public class Binary : Expression
    {
        public Binary(Expression left, Token @operator, Expression right)
        {
            this.Left = left;
            this.@Operator = @operator;
            this.Right = right;
        }

        public readonly Expression Left;
        public readonly Token @Operator;
        public readonly Expression Right;

        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitBinaryExp(this);
    }
    public class Grouping : Expression
    {
        public Grouping(Expression expression)
        {
            this.Expression = expression;
        }

        public readonly Expression Expression;

        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitGroupingExp(this);
    }
    public class Literal : Expression
    {
        public Literal(Object value)
        {
            this.Value = value;
        }

        public readonly Object Value;

        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitLiteralExp(this);
    }
    public class Unary : Expression
    {
        public Unary(Token @operator, Expression right)
        {
            this.@Operator = @operator;
            this.Right = right;
        }

        public readonly Token @Operator;
        public readonly Expression Right;

        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitUnaryExp(this);
    }
    public class Variable : Expression
    {
        public Variable(Token name)
        {
            this.Name = name;
        }

        public readonly Token Name;
        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitVariableExp(this);
    }
    public class Assign : Expression
    {
        public Assign(Token name, Expression value)
        {
            this.Name = name;
            this.Value = value;
        }

        public readonly Token Name;
        public readonly Expression Value;
        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitAssignExpr(this);
    }
    public class Logical : Expression
    {
        public Logical(Expression Left, Token @Operator, Expression Right)
        {
            this.Left = Left;
            this.@Operator = @Operator;
            this.Right = Right;
        }

        public readonly Expression Left;
        public readonly Token @Operator;
        public readonly Expression Right;
        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitLogicalExpression(this);
    }
    public class Call : Expression
    {
        public Call(Expression callee, Token paren, List<Expression> arguments)
        {
            this.Callee = callee;
            this.Paren = paren;
            this.Arguments = arguments;
        }

        public readonly Expression Callee;
        public readonly Token Paren;
        public readonly List<Expression> Arguments;
        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitCallExpression(this);
    }
}