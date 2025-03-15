using System.Text;

namespace Core.Core
{
    public class Environment
    {
        private Environment enclosing;
        public Environment()
        {
            enclosing = null;
        }
        public Environment(Environment enclosing)
        {
            this.enclosing = enclosing;
        }

        private Dictionary<string, object> map = new();

        public void Define(string name, object value) => map.Add(name, value);

        public object Get(Token name)
        {
            if (map.TryGetValue(name._lexeme, out var value))
                return value;

            if (enclosing is { }) return enclosing.Get(name);

            throw new Exception("Undefined variable");
        }
        public void Assign(Token name, Object value)
        {
            if (map.ContainsKey(name._lexeme))
            {
                map[name._lexeme] = value;
                return;
            }

            if (enclosing is { })
            {
                enclosing.Assign(name, value);
                return;
            }

            throw new Exception("Undefined variable");
        }
    }
}
