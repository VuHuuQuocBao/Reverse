using Compiler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core.Visitor
{
    public interface IVisitor<R>
    {
        public R VisitBinaryExp(Binary exp);
        public R VisitGroupingExp(Grouping exp);
        public R VisitUnaryExp(Unary exp);
        public R VisitLiteralExp(Literal exp);
    }
}


