using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.NativeTypes
{
    public class Integer
    {
        public static implicit operator Integer(int value) => new Integer(value);
        public static implicit operator int(Integer value) => value.Value;
        public static implicit operator double(Integer value) => value.Value;
        public static implicit operator float(Integer value) => value.Value;
        public static implicit operator long(Integer value) => value.Value;

        public int Value { get; set; }

        public Integer(int value)
        {
            Value = value;
        }
        public static Integer operator+(Integer lhs, Integer rhs)
        {
            return new Integer(lhs.Value + rhs.Value);
        }

        public static Integer operator -(Integer lhs, Integer rhs)
        {
            return new Integer(lhs.Value - rhs.Value);
        }

        public static Integer operator *(Integer lhs, Integer rhs)
        {
            return new Integer(lhs.Value * rhs.Value);
        }

        public static Integer operator /(Integer lhs, Integer rhs)
        {
            return new Integer(lhs.Value / rhs.Value);
        }

        public static Integer operator %(Integer lhs, Integer rhs)
        {
            return new Integer(lhs.Value % rhs.Value);
        }


        public static bool operator ==(Integer lhs, Integer rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(Integer lhs, Integer rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static bool operator >(Integer lhs, Integer rhs)
        {
            return lhs.Value > rhs.Value;
        }

        public static bool operator >=(Integer lhs, Integer rhs)
        {
            return lhs.Value >= rhs.Value;
        }

        public static bool operator <(Integer lhs, Integer rhs)
        {
            return lhs.Value < rhs.Value;
        }

        public static bool operator <=(Integer lhs, Integer rhs)
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

            if (obj.GetType() != typeof(Integer))
            {
                var integer = Convert.ChangeType(obj, typeof(int));
                if (integer is int i)
                {
                    return Value == i;
                }
                return false;
            }else
            {
                return Equals((Integer)obj);
            }
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
