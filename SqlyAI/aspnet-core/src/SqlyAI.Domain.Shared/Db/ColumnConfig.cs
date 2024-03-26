using AppCommon.Enums;

namespace SqlyAI.Domain.Db
{
    public class ColumnConfig
    {
        public string ColumnName { get; set; }
        public ColumnType ColumnType { get; set; }
    }
}