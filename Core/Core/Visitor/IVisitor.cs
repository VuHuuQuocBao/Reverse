﻿namespace Core.Core.Visitor
{
    public interface IExpressionVisitor<R>
    {
        public R VisitBinaryExp(Binary exp);
        public R VisitGroupingExp(Grouping exp);
        public R VisitUnaryExp(Unary exp);
        public R VisitLiteralExp(Literal exp);
        public R VisitVariableExp(Variable exp);
        public R VisitAssignExpr(Assign exp);
        public R VisitLogicalExpression(Logical exp);
    }

    public interface IStatementVisitor<R>
    {
        public R VisitPrintStatement(PrintStatement stmt);
        public R VisitExpressionStatement(ExpressionStatement stmt);
        public R VisitVarStatement(VarStatement stmt);
        public R VisitBlockStatement(BlockStatement stmt);
        public R VisitIfStatement(IfStatement stmt);
        public R VisitWhileStatement(WhileStatement stmt);
    }

}


