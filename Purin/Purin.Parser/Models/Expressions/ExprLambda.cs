using Purin.Parser.Interfaces;
using Purin.Parser.Models.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Expressions
{
    public class ExprLambda : BaseExpression
    {
      public class ExprLambdaParameter
        {
            public string Name { get; set; } = "";
            public BaseExpression TypeName { get; set; }
            public ExprLambdaParameter(string name, BaseExpression type)
            {
                Name = name;
                TypeName = type;
            }
        }
        public string Name { get; set; }
        public List<ExprLambdaParameter> Parameters { get; set; }
        public List<BaseStatement> Statements { get; set; }
        public BaseExpression ReturnType { get; set; }
        public ExprLambda(string name, List<ExprLambdaParameter> parameters, List<BaseStatement> statements, BaseExpression returnType, Location loc)
            : base("ExprLambda", loc)
        {
            Name = name;
            Parameters = parameters;
            Statements = statements;
            ReturnType = returnType;
        }

        public override object? Visit(IInterpreter interpreter)
        {
            return interpreter.Accept(this);
        }
    }
}
