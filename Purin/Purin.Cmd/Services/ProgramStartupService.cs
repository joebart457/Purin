using CliParser;
using Logger;
using Purin.Interpreter;
using Purin.NativeTypes;
using Purin.Parser;
using Purin.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Cmd.Services
{
    [Entry("purin")]
    internal class ProgramStartupService
    {
        private DefaultInterpreter _interpreter;
        private DefaultParser _parser;
        private RuntimeContext _runtime;
        private Dictionary<Type, Type> _typeTranslations;

        private bool _translateTypes;
        private bool _excludeBuiltins;
        public ProgramStartupService()
        {
            _runtime = CreateRuntime();
            _typeTranslations = CreateTypeTranslations();
            _interpreter = new DefaultInterpreter(_runtime, _typeTranslations);
            _parser = new DefaultParser();
        }

        private RuntimeContext CreateRuntime()
        {
            var runtime = new RuntimeContext();
            runtime.Register(typeof(Console));
            runtime.Register(typeof(File));
            runtime.Register(typeof(Path));
            runtime.Register(typeof(Directory));
            
            runtime.Register<ConsoleColor>();
            runtime.Register(typeof(List<>));
            runtime.Register(typeof(Dictionary<,>));

            if (_translateTypes)
            {
                runtime.Register<Purin.NativeTypes.String>();
                runtime.Register<Purin.NativeTypes.Integer>();
                runtime.Register<Purin.NativeTypes.Boolean>();
                runtime.Register<Purin.NativeTypes.Double>();
                runtime.Register<Purin.NativeTypes.Float>();
                runtime.Register<Purin.NativeTypes.UnsignedInteger>();
                runtime.Register<Purin.NativeTypes.Long>();
                if (!_excludeBuiltins)
                {
                    runtime.RegisterFullNamespaceOnly<string>();
                    runtime.RegisterFullNamespaceOnly<int>();
                    runtime.RegisterFullNamespaceOnly<bool>();
                    runtime.RegisterFullNamespaceOnly<double>();
                    runtime.RegisterFullNamespaceOnly<float>();
                    runtime.RegisterFullNamespaceOnly<uint>();
                    runtime.RegisterFullNamespaceOnly<long>();
                }
            }
            else
            {
                runtime.Register<string>();
                runtime.Register<int>();
                runtime.Register<bool>();
                runtime.Register<double>();
                runtime.Register<float>();
                runtime.Register<uint>();
                runtime.Register<long>();
            }

            return runtime;
        }

        private Dictionary<Type, Type> CreateTypeTranslations()
        {
            var typeTranslations = new Dictionary<Type, Type>();
            if (_translateTypes)
            {
                typeTranslations.Add(typeof(int), typeof(Purin.NativeTypes.Integer));
                typeTranslations.Add(typeof(double), typeof(Purin.NativeTypes.Double));
                typeTranslations.Add(typeof(float), typeof(Purin.NativeTypes.Float));
                typeTranslations.Add(typeof(uint), typeof(Purin.NativeTypes.UnsignedInteger));
                typeTranslations.Add(typeof(bool), typeof(Purin.NativeTypes.Boolean));
                typeTranslations.Add(typeof(long), typeof(Purin.NativeTypes.Long));
                typeTranslations.Add(typeof(string), typeof(Purin.NativeTypes.String));
            }         
            return typeTranslations;
        }

        private void Setup(bool translateTypes, bool excludeBuiltins)
        {
            _translateTypes = translateTypes;
            _excludeBuiltins = excludeBuiltins;
            _runtime = CreateRuntime();
            _typeTranslations = CreateTypeTranslations();
            _interpreter = new DefaultInterpreter(_runtime, _typeTranslations);
            _parser = new DefaultParser();
        }

        [Command("run")]
        public void Run(string scriptFile, string args, bool translateTypes = false, bool excludeBuiltins = false)
        {
            Setup(translateTypes, excludeBuiltins);
            var directives = _parser.Parse(File.ReadAllText(scriptFile));
            _interpreter.Run(directives);
            var entry = _interpreter.GetEntry();
            if (entry == null) CliLogger.LogWarning("entry point is not defined");
            else entry.Call(_interpreter, args.Split().Select(x => (object)x).ToArray());
        }

        [Command("repl")]
        public void Repl(bool translateTypes = false, bool excludeBuiltins = false, bool showExpandedErrors = false)
        {
            Setup(translateTypes, excludeBuiltins);
            bool exitRequested = false;
            while (!exitRequested)
            {
                CliLogger.Log(" > ", LogSeverity.LOGS, true, "");
                var line = Console.ReadLine() ?? "";
                if (line.ToLower() == "exit") break;
                if (string.IsNullOrWhiteSpace(line)) continue;
                try
                {
                    var directives = _parser.Parse(line);
                    _interpreter.Run(directives);

                } catch (Exception ex)
                {
                    CliLogger.LogError($"ERROR --- {(showExpandedErrors? ex.ToString() : ex.Message)}");
                }
            }
        }

        [Command("inspect")]
        public void Inspect(string scriptFile, bool translateTypes = false, bool excludeBuiltins = false)
        {
            Setup(translateTypes, excludeBuiltins);
            var directives = _parser.Parse(File.ReadAllText(scriptFile));
            _interpreter.Run(directives);

            InspectEnvironment(_runtime.Environment);
        }

        private static void InspectEnvironment(Runtime.Models.Environment environment, string pad = "")
        {
            foreach(var kv in environment.Lookup)
            {
                var key = $"{pad}{kv.Key} := ";
                CliLogger.LogInfo($"{key}{kv.Value}");
                if (kv.Value is Runtime.Models.Environment env)
                {
                    InspectEnvironment(env, pad + new string(' ', key.Length));
                }
            }
        }

        [Command("types")]
        public void DisplayTypes(string scriptFile, bool translateTypes = false, bool excludeBuiltins = false)
        {
            Setup(translateTypes, excludeBuiltins);
            var directives = _parser.Parse(File.ReadAllText(scriptFile));
            _interpreter.Run(directives);
            var types = _runtime.GetRegisteredTypes();
            var typeDictionary = new Dictionary<Type, List<string>>();
            foreach (var kv in types)
            {
                kv.Value.ForEach(ty =>
                {
                    if (typeDictionary.TryGetValue(ty, out var aliases))
                    {
                        aliases.Add(kv.Key);
                    } else
                    {
                        typeDictionary[ty] = new List<string>() { kv.Key };
                    }
                });
            }

            foreach (var kv in typeDictionary)
            {
                var keyString = $"{kv.Key.FullName ?? kv.Key.Name} := ";
                var typeNames = string.Join($"\n{new string(' ', keyString.Length)}", kv.Value);
                CliLogger.LogInfo($"{keyString}{typeNames}");
            }
        }
    }
}
