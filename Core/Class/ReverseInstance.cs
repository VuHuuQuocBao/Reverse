using Core.Core;

namespace Core.Class
{
    public class ReverseInstance
    {
        private ReverseClass Klass;
        private Dictionary<string, object> fields = new();

        public ReverseInstance(ReverseClass klass)
        {
            this.Klass = klass;
        }
        public object Get(Token name)
        {
            if (fields.ContainsKey(name._lexeme))
                return fields[name._lexeme];
            throw new Exception($"Undefined property '{name._lexeme}'.");
        }

        public void set(Token name, Object value)
        {
            if (!fields.TryAdd(name._lexeme, value))
                fields[name._lexeme] = value;
        }

    }

}
