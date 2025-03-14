namespace Core.Core.Visitor
{
    public interface IExpressionVisitor<R>
    {
        public R VisitBinaryExp(Binary exp);
        public R VisitGroupingExp(Grouping exp);
        public R VisitUnaryExp(Unary exp);
        public R VisitLiteralExp(Literal exp);
    }

    public interface IStatementVisitor<R>
    {
        public R VisitPrintStatement(PrintStatement stmt);
        public R VisitExpressionStatement(ExpressionStatement stmt);
        public R VisitVarStatement(VarStatement stmt);
    }

}


