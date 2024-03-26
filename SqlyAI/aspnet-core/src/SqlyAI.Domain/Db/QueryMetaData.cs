using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using System;

namespace SqlyAI.Domain.Db
{
    [Table("QueryMetaDatas")]
    public class QueryMetaData : FullAuditedAggregateRoot<Guid>
    {
        private List<ReportConfig> reportConfigs = [];
        private List<ColumnConfig> columnConfigs = [];
        private List<string> lastQueryColumns = [];

        public List<string> LastQueryColumns { get => lastQueryColumns ?? []; set => lastQueryColumns = value; }
        public List<string> AdditionColumns
        {
            get
            {
                var result = new List<string>();
                foreach (var jsonColumn in JsonColumns)
                {
                    result.AddRange(jsonColumn.Properties.Select(x => $"@json_{jsonColumn.ColumnName}_{x}"));
                }
                return result;
            }
        }

        public virtual Guid QueryId { get; set; }

        [ForeignKey("QueryId")]
        public Query Query { get; set; }

        public List<ColumnConfig> ColumnConfigs { get => columnConfigs ?? new List<ColumnConfig>(); set => columnConfigs = value; }
        public List<JsonColumnConfig> JsonColumns { get; set; } = new List<JsonColumnConfig>();


        public List<ReportConfig> ReportConfigs { get => reportConfigs ?? new List<ReportConfig>(); set => reportConfigs = value; }
        public string ExtraInfo { get; set; }

    }

}