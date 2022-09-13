using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Directives
{
    public class DirectiveEntry : BaseDirective
    {
        public BaseExpression Target { get; set; }
        public DirectiveEntry(BaseExpression target, Location loc)
            : base("DirectiveEntry", loc)
        {
            Target = target;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
