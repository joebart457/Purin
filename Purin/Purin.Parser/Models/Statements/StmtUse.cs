using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtUse : BaseStatement
    {
        public BaseExpression Path { get; set; };
        public StmtUse(BaseExpression path, Location loc)
            : base("StmtUse", loc)
        {
            Path = path;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
