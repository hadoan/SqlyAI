using System.Collections.Generic;

namespace SqlyAI.Domain.Db
{
    public class JsonColumnConfig
    {
        public string ColumnName { get; set; }
        public List<string> Properties { get; set; }
    }
}