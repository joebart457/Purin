using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Directives
{
    public class DirectiveProvideLib : BaseDirective
    {
        public string Path { get; set; } = "";
        public DirectiveProvideLib(string path, Location loc)
            : base("DirectiveProvideLib", loc)
        {
            Path = path;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
