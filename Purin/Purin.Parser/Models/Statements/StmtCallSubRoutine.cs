using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtCallSubRoutine : BaseStatement
    {
        public string Callee { get; set; }
        public IList<BaseExpression> Arguments { get; set; } = new List<BaseExpression>();
        public StmtCallSubRoutine(string callee, IList<BaseExpression> arguments, Location loc)
            : base("StmtCallSubRoutine", loc)
        {
            Callee = callee;
            Arguments = arguments;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
