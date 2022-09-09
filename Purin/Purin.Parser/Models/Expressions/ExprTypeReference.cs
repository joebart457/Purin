using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprTypeReference : BaseExpression
    {
        public string Path { get; set; }
        public ExprTypeReference(string path, Location loc)
            : base("ExprTypeReference", loc)
        {
            Path = path;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
