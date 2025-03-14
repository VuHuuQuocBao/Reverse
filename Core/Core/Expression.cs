﻿using Core.Core.Visitor;


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
            this.left = left;
            this.@operator = @operator;
            this.right = right;
        }

        public readonly Expression left;
        public readonly Token @operator;
        public readonly Expression right;

        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitBinaryExp(this);
    }
    public class Grouping : Expression
    {
        public Grouping(Expression expression)
        {
            this.expression = expression;
        }

        public readonly Expression expression;

        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitGroupingExp(this);
    }
    public class Literal : Expression
    {
        public Literal(Object value)
        {
            this.value = value;
        }

        public readonly Object value;

        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitLiteralExp(this);
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

        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitUnaryExp(this);
    }
    public class Variable : Expression
    {
        public Variable(Token name)
        {
            this.name = name;
        }

        public readonly Token name;
        public override T Accept<T>(IExpressionVisitor<T> visitor) => visitor.VisitVariableExp(this);
    }
    public class Assign : Expression
    {
        public Assign(Token name, Expression value)
        {
            this.name = name;
            this.value = value;
        }

        public readonly Token name;
        public readonly Expression value;
        public override T Accept<T>(IExpressionVisitor<T> visitor) => throw new NotImplementedException();
    }
}