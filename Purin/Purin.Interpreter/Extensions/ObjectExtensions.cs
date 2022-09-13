using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Interpreter.Extensions
{
    internal static class ObjectExtensions
    {
        public static object? ChangeType(this object? value, Type type)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.GetType() == type || type == typeof(object) || type.IsAssignableFrom(value.GetType())) return value;

            var conversion = type.GetImplicitConversionOrNull(value.GetType(), out var declaringType);
            if (conversion == null)
            {
                if (type is IConvertible convertible)
                {
                    return Convert.ChangeType(value, type);
                }
                throw new Exception($"unable to find conversion function from type {value.GetType().Name} to type {type.Name}");
            }

            return conversion.Invoke(declaringType, new object[] { value });
        }
    }
}
