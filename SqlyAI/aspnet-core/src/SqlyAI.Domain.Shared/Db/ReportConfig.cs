using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlyAI.Domain.Db;
public class ReportConfig
    {

        [NotMapped]
        public ReportType Type { get; set; }

        public string XAxisColumn { get; set; }

        [NotMapped]
        public CalculationMethod XAxisCalculationMethod { get; set; }

        public string X1AxisColumn { get; set; }

        [NotMapped]
        public CalculationMethod X1AxisCalculationMethod { get; set; }


        public string YAxisColumn { get; set; }

        [NotMapped]
        public CalculationMethod YAxisCalculationMethod { get; set; }

        public string Y1AxisColumn { get; set; }

        [NotMapped]
        public CalculationMethod Y1AxisCalculationMethod { get; set; }
        public string Y2AxisColumn { get; set; }

        [NotMapped]
        public CalculationMethod Y2AxisCalculationMethod { get; set; }
        public string Y3AxisColumn { get; set; }

        [NotMapped]
        public CalculationMethod Y3AxisCalculationMethod { get; set; }
        public string Y4AxisColumn { get; set; }

        [NotMapped]
        public CalculationMethod Y4AxisCalculationMethod { get; set; }
        public string Y5AxisColumn { get; set; }

        [NotMapped]
        public CalculationMethod Y5AxisCalculationMethod { get; set; }
    }
