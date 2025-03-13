using Compiler.Core;
using Core.Core.Visitor;


namespace Compiler.Core
{
    abstract public class Expression
    {
        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}
public class Binary : Expression
{
    public Binary(Expression left, Token @operator, Expression right)
    {
        this.left = left;
        this.@operator = @operator;
        this.right = right;
    }

    public readonly Expression left;
    public readonly Token @operator;
    public readonly Expression right;

    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitBinaryExp(this);
}
public class Grouping : Expression
{
    public Grouping(Expression expression)
    {
        this.expression = expression;
    }

    public readonly Expression expression;

    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitGroupingExp(this);
}
public class Literal : Expression
{
    public Literal(Object value)
    {
        this.value = value;
    }

    public readonly Object value;

    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitLiteralExp(this);
}
public class Unary : Expression
{
    public Unary(Token @operator, Expression right)
    {
        this.@operator = @operator;
        this.right = right;
    }

    public readonly Token @operator;
    public readonly Expression right;

    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitUnaryExp(this);
}
