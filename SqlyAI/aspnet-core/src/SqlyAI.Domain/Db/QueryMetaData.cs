//using System.ComponentModel.DataAnnotations.Schema;
//using Abp.Domain.Entities.Auditing;
//using System.Collections.Generic;
//using Abp.Organizations;
//using System.Linq;
//using Microsoft.Extensions.Azure;
//using Ardalis.SmartEnum;
//using System.Security.Policy;
//using Newtonsoft.Json;
//using Ardalis.SmartEnum.JsonNet;
//using Microsoft.AspNetCore.Components.Forms;

//namespace App.Db
//{
//    [Table("QueryMetaDatas")]
//    public class QueryMetaData : FullAuditedEntity<long>
//    {
//        private List<ReportConfig> reportConfigs = new List<ReportConfig>();
//        private List<ColumnConfig> columnConfigs = new List<ColumnConfig>();
//        private List<string> lastQueryColumns = new List<string>();

//        public List<string> LastQueryColumns { get => lastQueryColumns ?? new List<string>(); set => lastQueryColumns = value; }
//        public List<string> AdditionColumns
//        {
//            get
//            {
//                var result = new List<string>();
//                foreach (var jsonColumn in JsonColumns)
//                {
//                    result.AddRange(jsonColumn.Properties.Select(x => $"@json_{jsonColumn.ColumnName}_{x}"));
//                }
//                return result;
//            }
//        }

//        public virtual long QueryId { get; set; }

//        [ForeignKey("QueryId")]
//        public Query Query { get; set; }

//        public List<ColumnConfig> ColumnConfigs { get => columnConfigs ?? new List<ColumnConfig>(); set => columnConfigs = value; }
//        public List<JsonColumnConfig> JsonColumns { get; set; } = new List<JsonColumnConfig>();


//        public List<ReportConfig> ReportConfigs { get => reportConfigs ?? new List<ReportConfig>(); set => reportConfigs = value; }
//        public string ExtraInfo { get; set; }

//    }

//    public class ReportType : SmartEnum<ReportType>
//    {
//        private ReportType(string name, int value) : base(name, value)
//        {
//        }
//        public static readonly ReportType Bar = new ReportType(nameof(Bar), 1);
//        public static readonly ReportType Line = new ReportType(nameof(Line), 2);
//        public static readonly ReportType Doughnut = new ReportType(nameof(Doughnut), 3);
//        public static readonly ReportType Polar = new ReportType(nameof(Polar), 4);
//        public static readonly ReportType Pie = new ReportType(nameof(Pie), 4);
//        public static readonly ReportType Radar = new ReportType(nameof(Radar), 4);
//        public static readonly ReportType Combo = new ReportType(nameof(Combo), 4);

//        public static readonly List<ReportType> All = new List<ReportType>() { ReportType.Bar, ReportType.Line, ReportType.Doughnut, Polar, Pie, Radar, Combo };
//    }

//    public class CalculationMethod : SmartEnum<CalculationMethod>
//    {
//        private CalculationMethod(string name, int value) : base(name, value)
//        {
//        }

//        public static readonly CalculationMethod Distinct = new CalculationMethod(nameof(Distinct), 1);
//        public static readonly CalculationMethod Count = new CalculationMethod(nameof(Count), 2);
//        public static readonly CalculationMethod Sum = new CalculationMethod(nameof(Sum), 3);
//        public static readonly CalculationMethod Average = new CalculationMethod(nameof(Average), 4);
//        public static readonly CalculationMethod Min = new CalculationMethod(nameof(Min), 5);
//        public static readonly CalculationMethod Max = new CalculationMethod(nameof(Max), 6);
//    }
//    public class ReportConfig
//    {

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<ReportType, int>))]
//        public ReportType Type { get; set; }

//        public string XAxisColumn { get; set; }

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<CalculationMethod, int>))]
//        public CalculationMethod XAxisCalculationMethod { get; set; }

//        public string X1AxisColumn { get; set; }

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<CalculationMethod, int>))]
//        public CalculationMethod X1AxisCalculationMethod { get; set; }


//        public string YAxisColumn { get; set; }

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<CalculationMethod, int>))]
//        public CalculationMethod YAxisCalculationMethod { get; set; }

//        public string Y1AxisColumn { get; set; }

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<CalculationMethod, int>))]
//        public CalculationMethod Y1AxisCalculationMethod { get; set; }
//        public string Y2AxisColumn { get; set; }

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<CalculationMethod, int>))]
//        public CalculationMethod Y2AxisCalculationMethod { get; set; }
//        public string Y3AxisColumn { get; set; }

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<CalculationMethod, int>))]
//        public CalculationMethod Y3AxisCalculationMethod { get; set; }
//        public string Y4AxisColumn { get; set; }

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<CalculationMethod, int>))]
//        public CalculationMethod Y4AxisCalculationMethod { get; set; }
//        public string Y5AxisColumn { get; set; }

//        [NotMapped]
//        [JsonConverter(typeof(SmartEnumNameConverter<CalculationMethod, int>))]
//        public CalculationMethod Y5AxisCalculationMethod { get; set; }
//    }

//    public class JsonColumnConfig
//    {
//        public string ColumnName { get; set; }
//        public List<string> Properties { get; set; }
//    }

//    public class ColumnConfig
//    {
//        public string ColumnName { get; set; }
//        public string ColumnType { get; set; }
//    }
//}