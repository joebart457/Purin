using Purin.Runtime.Extensions;
using Purin.Runtime.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Environment = Purin.Runtime.Models.Environment;

namespace Purin.Runtime
{
    public class RuntimeContext
    {
        public Environment Environment { get; set; } = new Environment();
        private Dictionary<string, List<Type>> _types = new Dictionary<string, List<Type>>();
        public RuntimeContext()
        {

        }

        public void RegisterAssembly(string pathToAssembly)
        {
            var asm = Assembly.LoadFrom(pathToAssembly);
            if (asm == null) throw new PurinLoadException($"unable to load assembly from path {pathToAssembly}");
            var types = asm.GetExportedTypes();
            foreach (var type in types)
            {
                Register(type);
            }
        }

        public void Register<Ty>()
        {
            Register(typeof(Ty));
        }
        public void Register(Type type)
        {
            AddClassDefinition(type);
            AddToEnvironment(type);
        }

        public void RegisterFullNamespaceOnly<Ty>()
        {
            RegisterFullNamespaceOnly(typeof(Ty));
        }

        public void RegisterFullNamespaceOnly(Type type)
        {
            AddToDefinitions(type.FullName, type);
            AddToDefinitions(type.AssemblyQualifiedName, type);
            AddToEnvironment(type);
        }

        public Type GetType(string typeName)
        {
            if (!_types.TryGetValue(typeName, out var result) || result == null || !result.Any())
                throw new Exception($"{typeName} is not a valid type");
            if (result.Count > 1) throw new PurinRuntimeException($"reference to type {typeName} is ambiguous. Please use the fully qualified name");
            return result[0];
        }

        public Dictionary<string, List<Type>> GetRegisteredTypes()
        {
            return _types;
        }
        private void AddClassDefinition(Type type)
        {
            AddToDefinitions(type.Name, type);
            AddToDefinitions(type.FullName, type);
            AddToDefinitions(type.AssemblyQualifiedName, type);
        }

        private void AddToDefinitions(string? key, Type type)
        {
            if (key == null) return;
            if (_types.TryGetValue(key, out var tys) && tys != null && tys.Any())
            {
                if (!tys.Contains(type)) tys.Add(type);
                return;
            }
            _types.Add(key, new List<Type> { type });
        }

        private void AddToEnvironment(Type type)
        {
            Environment.AddTypeDefinition(type);
        }

    }
}
