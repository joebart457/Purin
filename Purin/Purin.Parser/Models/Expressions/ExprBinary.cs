using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprBinary : BaseExpression
    {
        public string Operator { get; set; }
        public BaseExpression Lhs { get; set; }
        public BaseExpression Rhs { get; set; }

        public ExprBinary(string op, BaseExpression lhs, BaseExpression rhs, Location loc)
            : base("ExprBinary", loc)
        {
            Operator = op;
            Lhs = lhs;
            Rhs = rhs;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
