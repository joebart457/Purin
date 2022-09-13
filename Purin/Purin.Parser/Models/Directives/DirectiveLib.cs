using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Directives
{
    public class DirectiveLib : BaseDirective
    {
        public string Path { get; set; } = "";
        public DirectiveLib(string path, Location loc)
            : base("DirectiveLib", loc)
        {
            Path = path;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
