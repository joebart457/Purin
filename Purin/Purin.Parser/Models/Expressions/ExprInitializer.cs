using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprInitializer : BaseExpression
    {
        public BaseExpression Type { get; set; }
        public IList<BaseExpression> Arguments { get; set; } = new List<BaseExpression>();

        public ExprInitializer(BaseExpression type, IList<BaseExpression> arguments, Location loc)
            : base("ExprInitializer", loc)
        {
            Type = type;
            Arguments = arguments;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
