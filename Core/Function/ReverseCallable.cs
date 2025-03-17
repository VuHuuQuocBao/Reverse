using Core.Core;
using Core.Core.Visitor;

namespace Core.Function
{
    public class ReverseCallable : IReverseCallable
    {
        private readonly Core.FunctionStatement _declaration;
        public ReverseCallable(Core.FunctionStatement declaration)
        {
            this._declaration = declaration;
        }
        public int Arity() => _declaration.Params.Count;
        public object Call(Interpreter interpreter, List<object> arguments)
        {
            Core.Environment environment = new Core.Environment();
            for (int i = 0; i < _declaration.@Params.Count; i++)
                environment.Define(_declaration.@Params[i]._lexeme, arguments[i]);
            interpreter.ExecuteBlock(_declaration.Body, environment);
            return null;
        }
    }
}
