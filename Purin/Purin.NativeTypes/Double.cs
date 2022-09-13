using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.NativeTypes
{
    public class Double
    {
        public static implicit operator Double(int value) => new Double(value);
        public static implicit operator double(Double value) => value.Value;
        public double Value { get; set; }

        public Double(double value)
        {
            Value = value;
        }
        public static Double operator +(Double lhs, Double rhs)
        {
            return new Double(lhs.Value + rhs.Value);
        }

        public static Double operator -(Double lhs, Double rhs)
        {
            return new Double(lhs.Value - rhs.Value);
        }

        public static Double operator *(Double lhs, Double rhs)
        {
            return new Double(lhs.Value * rhs.Value);
        }

        public static Double operator /(Double lhs, Double rhs)
        {
            return new Double(lhs.Value / rhs.Value);
        }

        public static Double operator %(Double lhs, Double rhs)
        {
            return new Double(lhs.Value % rhs.Value);
        }


        public static bool operator ==(Double lhs, Double rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(Double lhs, Double rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static bool operator >(Double lhs, Double rhs)
        {
            return lhs.Value > rhs.Value;
        }

        public static bool operator >=(Double lhs, Double rhs)
        {
            return lhs.Value >= rhs.Value;
        }

        public static bool operator <(Double lhs, Double rhs)
        {
            return lhs.Value < rhs.Value;
        }

        public static bool operator <=(Double lhs, Double rhs)
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

            if (obj.GetType() != typeof(Double))
            {
                var tyDouble = Convert.ChangeType(obj, typeof(double));
                if (tyDouble is double d)
                {
                    return Value == d;
                }
                return false;
            }
            else
            {
                return Equals((Double)obj);
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
