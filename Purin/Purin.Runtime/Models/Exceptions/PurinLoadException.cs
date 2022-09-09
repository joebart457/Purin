using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Runtime.Models.Exceptions
{
    public class PurinLoadException : PurinRuntimeException
    {
        public PurinLoadException(string message)
            : base(message)
        {

        }
    }
}
