using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Interpreter.Models.Exceptions
{
    internal class BreakException : System.Exception
    {
        public BreakException()
            : base("BreakException")
        {
        }
    }
}
