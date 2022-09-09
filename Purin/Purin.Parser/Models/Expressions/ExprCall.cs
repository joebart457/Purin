using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprCall : BaseExpression
    {
        public BaseExpression Callee { get; set; };
        public IList<BaseExpression> Arguments { get; set; } = new List<BaseExpression>();
        public ExprCall(BaseExpression callee, IList<BaseExpression> arguments, Location loc)
            : base("ExprCall", loc)
        {
            Callee = callee;
            Arguments = arguments;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
