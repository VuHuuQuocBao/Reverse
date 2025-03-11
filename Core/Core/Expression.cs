using Compiler.Core;
namespace Compiler.Core
{
    abstract class Expression
    {
    }
}
class Binary : Expression
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
}
class Grouping : Expression
{
    public Grouping(Expression expression)
    {
        this.expression = expression;
    }

    public readonly Expression expression;
}
class Literal : Expression
{
    public Literal(Object value)
    {
        this.value = value;
    }

    public readonly Object value;
}
class Unary : Expression
{
    public Unary(Token @operator, Expression right)
    {
        this.@operator = @operator;
        this.right = right;
    }

    public readonly Token @operator;
    public readonly Expression right;
}
