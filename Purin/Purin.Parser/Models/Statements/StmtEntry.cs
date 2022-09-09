using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtEntry : BaseStatement
    {
        public BaseExpression Target { get; set; }
        public StmtEntry(BaseExpression target, Location loc)
            : base("StmtEntry", loc)
        {
            Target = target;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
