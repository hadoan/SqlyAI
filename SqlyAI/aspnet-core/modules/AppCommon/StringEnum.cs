namespace AppCommon
{
    public class StringEnum
    {
        protected StringEnum(string value) { Value = value; }
        public string Value { get; }
        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return Value.Equals(obj?.ToString());
        }
    }
}
