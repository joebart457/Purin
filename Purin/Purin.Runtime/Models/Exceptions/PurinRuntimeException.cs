using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Runtime.Models.Exceptions
{
    public class PurinRuntimeException: System.Exception
    {
        public PurinRuntimeException(string message)
            :base(message)
        {
            
        }
    }
}
