using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.NativeTypes
{
    public class UnsignedInteger
    {
        public static implicit operator UnsignedInteger(uint value) => new UnsignedInteger(value);
        public static implicit operator uint(UnsignedInteger value) => value.Value;
        public static implicit operator double(UnsignedInteger value) => value.Value;
        public static implicit operator float(UnsignedInteger value) => value.Value;
        public static implicit operator long(UnsignedInteger value) => value.Value;

        public uint Value { get; set; }

        public UnsignedInteger(uint value)
        {
            Value = value;
        }
        public static UnsignedInteger operator +(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return new UnsignedInteger(lhs.Value + rhs.Value);
        }

        public static UnsignedInteger operator -(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return new UnsignedInteger(lhs.Value - rhs.Value);
        }

        public static UnsignedInteger operator *(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return new UnsignedInteger(lhs.Value * rhs.Value);
        }

        public static UnsignedInteger operator /(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return new UnsignedInteger(lhs.Value / rhs.Value);
        }

        public static UnsignedInteger operator %(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return new UnsignedInteger(lhs.Value % rhs.Value);
        }


        public static bool operator ==(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static bool operator >(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return lhs.Value > rhs.Value;
        }

        public static bool operator >=(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return lhs.Value >= rhs.Value;
        }

        public static bool operator <(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return lhs.Value < rhs.Value;
        }

        public static bool operator <=(UnsignedInteger lhs, UnsignedInteger rhs)
        {
            return lhs.Value <= rhs.Value;
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

            if (obj.GetType() != typeof(UnsignedInteger))
            {
                var UnsignedInteger = Convert.ChangeType(obj, typeof(uint));
                if (UnsignedInteger is uint i)
                {
                    return Value == i;
                }
                return false;
            }
            else
            {
                return Equals((UnsignedInteger)obj);
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
