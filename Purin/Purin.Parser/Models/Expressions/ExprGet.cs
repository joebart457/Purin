using Purin.Parser.Interfaces;
using Purin.Parser.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprGet : BaseExpression
    {
        public RetrievalType RetrievalType { get; set; }
        public BaseExpression Lhs { get; set; }
        public string Name { get; set; } = "";
        public ExprGet(BaseExpression lhs, RetrievalType retrievalType, string name, Location loc)
            : base("ExprGet", loc)
        {
            Lhs = lhs;
            RetrievalType = retrievalType;
            Name = name;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
