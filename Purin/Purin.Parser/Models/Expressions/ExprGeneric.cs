using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprGeneric : BaseExpression
    {
        public BaseExpression Lhs { get; set; }
        public List<BaseExpression> TypeArguments { get; set; }
        public ExprGeneric(BaseExpression lhs, List<BaseExpression> typeArguments, Location loc)
            : base("ExprGeneric", loc)
        {
            Lhs = lhs;
            TypeArguments = typeArguments;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
