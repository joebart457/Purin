using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Interpreter.Interfaces
{
    public interface ISubRoutine
    {
        void Call(DefaultInterpreter interpreter, object?[] args);
    }
}
