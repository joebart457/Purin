using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtWhile : BaseStatement
    {
        public BaseExpression Condition { get; set; }
        public BaseStatement Then { get; set; }
        public StmtWhile(BaseExpression condition, BaseStatement then, Location loc)
            : base("StmtWhile", loc)
        {
            Condition = condition;
            Then = then;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
