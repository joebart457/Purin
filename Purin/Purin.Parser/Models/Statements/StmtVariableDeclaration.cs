using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtVariableDeclaration : BaseStatement
    {
        public string Target { get; set; }
        public BaseExpression Value { get; set; }
        public StmtVariableDeclaration(string target, BaseExpression value, Location loc)
            : base("StmtVariableDeclaration", loc)
        {
            Target = target;
            Value = value;

        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
