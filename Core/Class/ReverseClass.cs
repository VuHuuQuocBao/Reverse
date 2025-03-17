using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public class ReverseClass
    {
        private readonly string name;

        public ReverseClass(string name)
        {
            this.name = name;
        }

        public override string ToString() => name;
    }

}
