using Purin.Interpreter.Interfaces;
using Purin.Interpreter.Models.Exceptions;
using Purin.Parser.Interfaces;
using Purin.Parser.Models.Statements;
using Purin.Runtime.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Interpreter.Models
{
    internal class SubRoutine : ISubRoutine
    {
        public class Parameter
        {
            public Type Type { get; set; }
            public string Name { get; set; }
            public object? Value { get; set; }
            public bool HasDefault { get; set; }
            public Parameter(Type type, string name, bool hasDefault, object? value = null)
            {
                Type = type;
                Name = name;
                HasDefault = hasDefault;
                Value = value;
            }
        }
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }
        public List<BaseStatement> Statements { get; set; }
        public SubRoutine(string name, List<Parameter> parameters, List<BaseStatement> statements)
        {
            Name = name;
            Parameters = parameters;
            Statements = statements;
        }

        public void Call(DefaultInterpreter interpreter, object?[] args)
        {
            if (args.Length > Parameters.Count) throw new PurinRuntimeException($"parity mismatch in call {Name}");

            var previous = interpreter.Environment;
            interpreter.Environment = new Runtime.Models.Environment(previous);
            Exception? err = null;
            try
            {
                int i = 0;
                for (; i < args.Length; i++)
                {
                    if (Parameters[i].Type == args[i]?.GetType())
                    {
                        interpreter.Environment.Define(Parameters[i].Name, args[i]);
                    }
                    else throw new PurinRuntimeException($"parameter type mismatch: expected {Parameters[i].Type.Name} but got {args[i]?.GetType().Name}");
                }

                // for the remining parameters, try to use their default value
                for (; i < Parameters.Count; i++)
                {
                    if (!Parameters[i].HasDefault) throw new PurinRuntimeException($"parameter {Parameters[i].Name} must have value provided");
                    interpreter.Environment.Define(Parameters[i].Name, Parameters[i].Value);
                }
                foreach (var stmt in Statements)
                {
                    interpreter.Accept(stmt);
                }
            }
            catch (Exception ex)
            {
                err = ex;
            }
            interpreter.Environment = previous;
            if (err != null) throw err;
        }
    }
}
