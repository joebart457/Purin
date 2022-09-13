using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.NativeTypes
{
    public class Float
    {
        public static implicit operator Float(int value) => new Float(value);
        public static implicit operator double(Float value) => value.Value;
        public static implicit operator float(Float value) => value.Value;

        public float Value { get; set; }

        public Float(float value)
        {
            Value = value;
        }
        public static Float operator +(Float lhs, Float rhs)
        {
            return new Float(lhs.Value + rhs.Value);
        }

        public static Float operator -(Float lhs, Float rhs)
        {
            return new Float(lhs.Value - rhs.Value);
        }

        public static Float operator *(Float lhs, Float rhs)
        {
            return new Float(lhs.Value * rhs.Value);
        }

        public static Float operator /(Float lhs, Float rhs)
        {
            return new Float(lhs.Value / rhs.Value);
        }

        public static Float operator %(Float lhs, Float rhs)
        {
            return new Float(lhs.Value % rhs.Value);
        }


        public static bool operator ==(Float lhs, Float rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(Float lhs, Float rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static bool operator >(Float lhs, Float rhs)
        {
            return lhs.Value > rhs.Value;
        }

        public static bool operator >=(Float lhs, Float rhs)
        {
            return lhs.Value >= rhs.Value;
        }

        public static bool operator <(Float lhs, Float rhs)
        {
            return lhs.Value < rhs.Value;
        }

        public static bool operator <=(Float lhs, Float rhs)
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

            if (obj.GetType() != typeof(Float))
            {
                var Float = Convert.ChangeType(obj, typeof(float));
                if (Float is float f)
                {
                    return Value == f;
                }
                return false;
            }
            else
            {
                return Equals((Float)obj);
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
