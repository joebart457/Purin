using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtLib : BaseStatement
    {
        public string Path { get; set; } = "";
        public StmtLib(string path, Location loc)
            : base("StmtLib", loc)
        {
            Path = path;    
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
