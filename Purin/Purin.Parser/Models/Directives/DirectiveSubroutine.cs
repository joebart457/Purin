using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using Purin.Parser.Models.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Directives
{
    public class DirectiveSubroutine : BaseDirective
    {
        public class DirectiveSubroutineParameter
        {
            public string Name { get; set; } = "";
            public BaseExpression TypeName { get; set; }
            public BaseExpression? Value { get; set; }
            public DirectiveSubroutineParameter(string name, BaseExpression type, BaseExpression? value = null)
            {
                Name = name;
                TypeName = type;
                Value = value;
            }
        }
        public string Name { get; set; }
        public List<DirectiveSubroutineParameter> Parameters { get; set; }
        public List<BaseStatement> Statements { get; set; }

        public DirectiveSubroutine(string name, List<DirectiveSubroutineParameter> parameters, List<BaseStatement> statements, Location loc)
            : base("DirectiveSubroutine", loc)
        {
            Name = name;
            Parameters = parameters;
            Statements = statements;
        }

        public override void Visit(IInterpreter interpreter)
        {
            interpreter.Accept(this);
        }
    }
}
