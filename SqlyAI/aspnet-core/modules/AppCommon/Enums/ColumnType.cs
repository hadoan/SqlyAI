using Ardalis.SmartEnum;

namespace App.Db
{
    public sealed class ColumnType : SmartEnum<ColumnType>
    {
        public static readonly ColumnType STRING = new ColumnType("string", 1);
        public static readonly ColumnType DATE = new ColumnType("date", 2);
        public static readonly ColumnType NUMBER = new ColumnType("number", 3);
        public static readonly ColumnType JSON = new ColumnType("json", 10);
        public static readonly ColumnType OTHER = new ColumnType("other", 100);

        private ColumnType(string name, int value) : base(name, value)
        {
        }

        public static ColumnType ParseName(string name)
        {
            try
            {
                return FromName(name, true);
            }
            catch (SmartEnumNotFoundException)
            {
                return OTHER;
            }
            
        }
    }
}
