
namespace Purin.NativeTypes
{
    public class String
    {
        public static implicit operator String(string value) => new String(value);
        public static implicit operator string(String value) => value.Value;

        public string Value { get; set; }

        public String(string value)
        {
            Value = value;
        }
        public static String operator +(String lhs, String rhs)
        {
            return new String(lhs.Value + rhs.Value);
        }
        public static bool operator ==(String lhs, String rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(String lhs, String rhs)
        {
            return lhs.Value != rhs.Value;
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

            if (obj.GetType() != typeof(String))
            {
                var String = Convert.ChangeType(obj, typeof(string));
                if (String is string s)
                {
                    return Value == s;
                }
                return false;
            }
            else
            {
                return Equals((String)obj);
            }
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
