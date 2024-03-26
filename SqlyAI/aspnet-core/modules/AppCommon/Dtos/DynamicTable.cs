using AppCommon.Enums;
using System;
using System.Collections.Generic;

namespace AppCommon.Dtos
{

    public class DynamicTable : DynamicTable<int> { }

    public class DynamicTable<T>
    {
        public string Delimiter { get; set; }
        public string SqlTableName { get; set; }
        public string FileName { get; set; }
        public Guid FileId { get; set; }
        public string FileType { get; set; }

        public double FileSize { get; set; }
        public List<ColumnInfo> ColumnNames { get; set; } = new List<ColumnInfo>();
        public List<RowInfo<T>> Rows { get; set; } = new List<RowInfo<T>>();

        public string FileUrl { get; set; }
    }


    public class RowInfo<T>
    {
        public T RowId { get; set; }
        public string RawValue { get; set; }
        public List<ColumnValue> Values { get; set; } = new List<ColumnValue>();
    }

    public class ColumnValue
    {
        public string ColumnName { get; set; }
        public string RawValue { get; set; }
    }

    public class ColumnInfo
    {
        public string RawName { get; set; }
        public string ColumnName { get; set; }
        public int Order { get; set; }

        public ColumnType Type { get; set; }

    }

}
