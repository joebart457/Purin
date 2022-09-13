using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.NativeTypes
{
    public class Boolean
    {
        public static implicit operator Boolean(bool value) => new Boolean(value);
        public static implicit operator bool(Boolean value) => value.Value;

        public bool Value { get; set; }

        public Boolean(bool value)
        {
            Value = value;
        }
        public static Boolean operator !(Boolean rhs)
        {
            return new Boolean(!rhs.Value);
        }
        public static bool operator ==(Boolean lhs, Boolean rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(Boolean lhs, Boolean rhs)
        {
            return lhs.Value != rhs.Value;
        }


        // Custom operators

        public static bool op_And(Boolean lhs, Boolean rhs)
        {
            return lhs.Value && rhs.Value;
        }

        public static bool op_Or(Boolean lhs, Boolean rhs)
        {
            return lhs.Value || rhs.Value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (obj.GetType() != typeof(Boolean))
            {
                var Boolean = Convert.ChangeType(obj, typeof(bool));
                if (Boolean is bool b)
                {
                    return Value == b;
                }
                return false;
            }
            else
            {
                return Equals((Boolean)obj);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
