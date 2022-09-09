using Purin.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprIdentifier : BaseExpression
    {
        public string Symbol { get; set; } = "";
        public ExprIdentifier(string symbol, Location loc)
            : base("ExprIdentifier", loc)
        {
            Symbol = symbol;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
