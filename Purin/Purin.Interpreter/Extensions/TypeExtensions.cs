using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Interpreter.Extensions
{
    internal static class TypeExtensions
    {
        public static MethodInfo? GetImplicitConversionOrNull(this Type toType, Type fromType)
        {
            return toType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => mi.Name == "op_Implicit" && mi.ReturnType == toType)
                .FirstOrDefault(mi => {
                    ParameterInfo? pi = mi.GetParameters().FirstOrDefault();
                    return pi != null && pi.ParameterType == fromType;
                });
        }

        public static MethodInfo? GetImplicitConversionOrNull(this Type outputType, Type inputType, out Type? declaringType)
        {
            declaringType = null;
            var methods = inputType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => mi.Name == "op_Implicit" && mi.ReturnType == outputType).ToList();
            methods.AddRange(outputType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => mi.Name == "op_Implicit" && mi.ReturnType == outputType));

            var methodInfo =  methods.FirstOrDefault(mi => {
                    ParameterInfo? pi = mi.GetParameters().FirstOrDefault();
                    return pi != null && pi.ParameterType == inputType;
                });

            if (methodInfo != null) declaringType = methodInfo.DeclaringType;
            return methodInfo;
        }

        public static MethodInfo? GetBinaryOperator(this Type thisType, string operatorName, ref object rhs)
        {
            var rhsType = rhs.GetType();
            var rhsCopy = rhs;

            var method = thisType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => mi.Name == operatorName)
                .FirstOrDefault(mi => {
                    var pi = mi.GetParameters();
                    var typesMatch = pi[1].ParameterType == rhsType;
                    if (!typesMatch)
                    {
                        var conversionMethod = pi[1].ParameterType.GetImplicitConversionOrNull(rhsType, out var declaringType);
                        if (conversionMethod == null) return false;
                        rhsCopy = conversionMethod.Invoke(declaringType, new object[] { rhsCopy });
                        return true;
                    }
                    return true;
                });
            if (method != null) rhs = rhsCopy;
            return method;
        }

        public static MethodInfo? GetUnaryOperator(this Type thisType, string operatorName)
        {
            return thisType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => mi.Name == operatorName)
                .FirstOrDefault(mi =>
                {
                    var pi = mi.GetParameters().FirstOrDefault();
                    return pi != null && pi.ParameterType == thisType;
                });
        }

    }
}
