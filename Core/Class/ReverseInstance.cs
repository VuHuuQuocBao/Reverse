using Core.Core;
using Core.Function;

namespace Core.Class
{
    public class ReverseInstance
    {
        private ReverseClass klass;
        private Dictionary<string, object> fields = new();

        public ReverseInstance(ReverseClass klass)
        {
            this.klass = klass;
        }
        public object Get(Token name)
        {
            if (fields.ContainsKey(name._lexeme))
                return fields[name._lexeme];

            ReverseCallable method = klass.findMethod(name._lexeme);
            if (method != null) return method;

            throw new Exception($"Undefined property '{name._lexeme}'.");
        }

        public void set(Token name, Object value)
        {
            if (!fields.TryAdd(name._lexeme, value))
                fields[name._lexeme] = value;
        }

    }

}
