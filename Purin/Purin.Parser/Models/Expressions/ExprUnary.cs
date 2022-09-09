using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprUnary : BaseExpression
    {
        public string Operator { get; set; }
        public BaseExpression Rhs { get; set; }

        public ExprUnary(string op, BaseExpression rhs, Location loc)
            : base("ExprUnary", loc)
        {
            Operator = op;
            Rhs = rhs;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
