using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core
{
    public class Environment
    {
        private Dictionary<string, object> map = new();

        public void Define(string name, object value) 
        {
            map.Add(name, value);
        }

        public object Get(Token name)
        {
            if (map.TryGetValue(name._lexeme, out var value))
                return value;

            throw new Exception("Undefined variable");
        }
    }
}
