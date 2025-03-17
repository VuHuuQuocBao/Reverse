using Core.Core;
using Core.Core.Visitor;

namespace Core.Function
{
    public class ReverseCallable : IReverseCallable
    {
        private readonly Core.FunctionStatement _declaration;
        private readonly Core.Environment _closure;
        public ReverseCallable(Core.FunctionStatement declaration, Core.Environment closure)
        {
            this._closure = closure;
            this._declaration = declaration;
        }
        public int Arity() => _declaration.Params.Count;
        public object Call(Interpreter interpreter, List<object> arguments)
        {
            var environment = new Core.Environment(_closure);
            for (int i = 0; i < _declaration.@Params.Count; i++)
                environment.Define(_declaration.@Params[i]._lexeme, arguments[i]);

            try
            {
                interpreter.ExecuteBlock(_declaration.Body, environment);
            }
            catch (ReturnException ex)
            {
                return ex.value;
            }

            return null;
        }
    }
}
