using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Interpreter.Extensions
{
    internal static class MethodInfoExtensions
    {
        public static object? CleanArgumentsThenInvoke(this MethodInfo methodInfo, object? obj, object?[] args)
        {
            var parameters = methodInfo.GetParameters();
            var cleanedArguments = new List<object?>();
            for(int i = 0; i < parameters.Length; i++)
            {
                if (i >= args.Length) break;
                cleanedArguments.Add(args[i].ChangeType(parameters[i].ParameterType));
            }
            return methodInfo.Invoke(obj, cleanedArguments.ToArray());
        }
    }
}
