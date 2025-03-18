using Core.Core.Visitor;
using Core.Function;

namespace Core.Class
{
    public class ReverseClass : IReverseCallable
    {
        private readonly string name;
        private readonly Dictionary<string, ReverseCallable> methods;
        public ReverseClass(string name)
        {
            this.name = name;
        }

        public ReverseClass(String name, Dictionary<string, ReverseCallable> methods)
        {
            this.name = name;
            this.methods = methods;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            var instance = new ReverseInstance(this);
            return instance;
        }

        public int Arity() => 0;

        public ReverseCallable findMethod(String name)
        {
            if (methods.ContainsKey(name))
                return methods[name];

            return null;
        }
        public override string ToString() => name;
    }

}
