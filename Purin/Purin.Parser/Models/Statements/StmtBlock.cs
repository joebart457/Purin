using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtBlock : BaseStatement
    {
        public IList<BaseStatement> Statements { get; set; }

        public StmtBlock(IList<BaseStatement> statements, Location loc)
            : base("StmtBlock", loc)
        {
            Statements = statements;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
