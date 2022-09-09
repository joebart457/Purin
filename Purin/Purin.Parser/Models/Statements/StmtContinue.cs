using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtContinue : BaseStatement
    {
        public StmtContinue(Location loc)
            : base("StmtContinue", loc)
        {
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
