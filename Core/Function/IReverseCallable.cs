using Core.Core.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Function
{
    public interface IReverseCallable
    {
        public object Call(Interpreter interpreter, List<object> arguments);
        public int Arity();
    }

}
