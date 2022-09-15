using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Directives
{
    public class DirectiveDo : BaseDirective
    {
        public BaseExpression Target { get; set; }
        public DirectiveDo(BaseExpression target, Location loc)
            : base("DirectiveDo", loc)
        {
            Target = target;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
