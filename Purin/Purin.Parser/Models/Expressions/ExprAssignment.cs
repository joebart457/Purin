using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprAssignment : BaseExpression
    {
        public BaseExpression Lhs { get; set; }
        public BaseExpression Value { get; set; }
        public ExprAssignment(BaseExpression lhs, BaseExpression value, Location loc)
            : base("ExprAssignment", loc)
        {
            Lhs = lhs;
            Value = value;

        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
