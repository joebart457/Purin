using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprGroup : BaseExpression
    {
        public BaseExpression Expression { get; set; }

        public ExprGroup(BaseExpression expression, Location loc)
            : base("ExprGroup", loc)
        {
            Expression = expression;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
