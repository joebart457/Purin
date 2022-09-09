using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtIfElse : BaseStatement
    {
        public BaseExpression Condition { get; set; }
        public BaseStatement Then { get; set; }
        public BaseStatement? Else { get; set; }
        public StmtIfElse(BaseExpression condition, BaseStatement then, BaseStatement? elseDo, Location loc)
            : base("StmtIfElse", loc)
        {
            Condition = condition;
            Then = then;
            Else = elseDo;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
