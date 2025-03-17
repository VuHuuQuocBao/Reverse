using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core
{
    public class ReturnException : Exception
    {
        public object value;
        public ReturnException(object valueParm)
        {
            value = valueParm;
        }   
    }
}
