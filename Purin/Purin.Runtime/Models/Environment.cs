using Purin.Runtime.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Runtime.Models
{
    public class Environment
    {
        private Environment? _enclosing = null;
        private Dictionary<string, object?> _lookup = new Dictionary<string, object?>();
        public Dictionary<string, object?> Lookup { get { return _lookup; } }
        public Environment(Environment? enclosing = null)
        {
            _enclosing = enclosing;
        }

        public bool Exists(string key)
        {
            if (_lookup.ContainsKey(key)) return true;
            if (_enclosing != null) return _enclosing.Exists(key);
            return false;
        }
        public object? Get(string key)
        {
            if (_lookup.TryGetValue(key, out var value)) return value;
            if (_enclosing != null) return _enclosing.Get(key);
            throw new PurinRuntimeException($"key: {key} is not defined in the current context");
        }

        public void Define(string key, object? value)
        {
            if (_lookup.ContainsKey(key))
            {
                throw new PurinRuntimeException($"redefinition of key: {key} is not allowed");
            }
            _lookup[key] = value;
        }

        public void Set(string key, object? value)
        {
            var oldValue = Get(key);
            if (oldValue?.GetType() != value?.GetType())
                throw new PurinRuntimeException($"types: {oldValue?.GetType()} {value?.GetType()} do not match or cannot be interpolated");
            _lookup[key] = value;
        }

        public void AddTypeDefinition(Type type)
        {
            var namespaces = type.Namespace?.Split('.') ?? new string[0];
            var definitionTarget = this;
            foreach(var ns in namespaces)
            {
                if (!(definitionTarget.TryGetValue(ns, out Environment? env) && env != null))
                {
                    // We want to make sure not to pass anything to enclosing here
                    // this is a special case because these are namespaces,
                    // NOT objects inheriting properties
                    env = new Environment();
                    definitionTarget.Define(ns, env);
                }

                definitionTarget = env;
            }
            definitionTarget.Define(type.Name, type);
        }

        private bool TryGetValue<Ty>(string key, out Ty? val)
        {
            if (_lookup.TryGetValue(key, out var value) && value is Ty tyVal)
            {
                val = tyVal;
                return true;
            }
            val = default(Ty);
            return false;

        }
    }
}
