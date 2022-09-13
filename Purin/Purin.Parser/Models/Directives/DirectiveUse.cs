using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Directives
{
    public class DirectiveUse : BaseDirective
    {
        public BaseExpression Path { get; set; }
        public DirectiveUse(BaseExpression path, Location loc)
            : base("DirectiveUse", loc)
        {
            Path = path;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
