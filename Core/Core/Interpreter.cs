using Core.Core.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core
{
    public class Interpreter : IVisitor<Object>
    {
        public object VisitBinaryExp(Binary exp)
        {
            throw new NotImplementedException();
        }

        public object VisitGroupingExp(Grouping exp)
        {
            throw new NotImplementedException();
        }

        public object VisitLiteralExp(Literal exp) => exp.value;
        public object VisitUnaryExp(Unary exp)
        {
            throw new NotImplementedException();
        }
    }
}
