using SqlyAI.Domain.Db;

namespace SqlyAI.Application.Db.Dtos
{
    public class QueryMetaDataDto : EntityDto<long>
    {
        private List<ReportConfigDto> reportConfigurations = new List<ReportConfigDto>();

        public List<string> LastQueryColumns { get; set; } = new List<string>();
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


        public List<ColumnConfig> ColumnConfigs { get; set; } = new List<ColumnConfig>();

        public List<JsonColumnConfig> JsonColumns { get; set; } = new List<JsonColumnConfig>();

        public List<ReportConfigDto> ReportConfigurations { get => reportConfigurations; set => reportConfigurations = value; }
    }

    public class ReportConfigDto
    {

        public ReportType Type { get; set; }

        public string XAxisColumn { get; set; }

        public string XAxisCalculationMethod { get; set; }


        public string YAxisColumn { get; set; }


        public string YAxisCalculationMethod { get; set; }

    }
}
