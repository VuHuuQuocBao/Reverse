using Core.Core.Visitor;
using Core.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class ReverseClass : IReverseCallable
    {
        private readonly string name;

        public ReverseClass(string name)
        {
            this.name = name;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            var instance = new ReverseInstance(this);
            return instance;
        }

        public int Arity() => 0;


        public override string ToString() => name;
    }

}
