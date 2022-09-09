using Purin.Parser.Interfaces;
using Purin.Parser.Models;
using Purin.Parser.Models.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtProvideLib : BaseStatement
    {
        public string Path { get; set; } = "";
        public StmtProvideLib(string path, Location loc)
            : base("StmtProvideLib", loc)
        {
            Path = path;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
