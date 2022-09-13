using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.NativeTypes
{
    public class Long
    {
        public static implicit operator Long(int value) => new Long(value);
        public static implicit operator long(Long value) => value.Value;
        public static implicit operator double(Long value) => value.Value;
        public static implicit operator float(Long value) => value.Value;

        public long Value { get; set; }

        public Long(long value)
        {
            Value = value;
        }
        public static Long operator +(Long lhs, Long rhs)
        {
            return new Long(lhs.Value + rhs.Value);
        }

        public static Long operator -(Long lhs, Long rhs)
        {
            return new Long(lhs.Value - rhs.Value);
        }

        public static Long operator *(Long lhs, Long rhs)
        {
            return new Long(lhs.Value * rhs.Value);
        }

        public static Long operator /(Long lhs, Long rhs)
        {
            return new Long(lhs.Value / rhs.Value);
        }

        public static Long operator %(Long lhs, Long rhs)
        {
            return new Long(lhs.Value % rhs.Value);
        }


        public static bool operator ==(Long lhs, Long rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(Long lhs, Long rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static bool operator >(Long lhs, Long rhs)
        {
            return lhs.Value > rhs.Value;
        }

        public static bool operator >=(Long lhs, Long rhs)
        {
            return lhs.Value >= rhs.Value;
        }

        public static bool operator <(Long lhs, Long rhs)
        {
            return lhs.Value < rhs.Value;
        }

        public static bool operator <=(Long lhs, Long rhs)
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

            if (obj.GetType() != typeof(Long))
            {
                var Long = Convert.ChangeType(obj, typeof(long));
                if (Long is long i)
                {
                    return Value == i;
                }
                return false;
            }
            else
            {
                return Equals((Long)obj);
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
