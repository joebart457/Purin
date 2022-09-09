using Purin.Parser.Interfaces;
using Purin.Parser.Models.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Statements
{
    public class StmtSubRoutine: BaseStatement
    {
        public class StmtSubRoutineParameter
        {
            public string Name { get; set; } = "";
            public BaseExpression TypeName { get; set; }
            public BaseExpression? Value { get; set; }
            public StmtSubRoutineParameter(string name, BaseExpression type, BaseExpression? value = null)
            {
                Name = name;
                TypeName = type;
                Value = value;
            }
        }
        public string Name { get; set; }
        public List<StmtSubRoutineParameter> Parameters { get; set; }
        public List<BaseStatement> Statements { get; set; }

        public StmtSubRoutine(string name, List<StmtSubRoutineParameter> parameters, List<BaseStatement> statements, Location loc)
            : base("StmtSubRoutine", loc)
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
