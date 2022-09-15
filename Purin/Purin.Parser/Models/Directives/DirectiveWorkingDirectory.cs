using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Directives
{
    public class DirectiveWorkingDirectory : BaseDirective
    {
        public string Path { get; set; } = "";
        public DirectiveWorkingDirectory(string path, Location loc)
            : base("DirectiveWorkingDirectory", loc)
        {
            Path = path;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
