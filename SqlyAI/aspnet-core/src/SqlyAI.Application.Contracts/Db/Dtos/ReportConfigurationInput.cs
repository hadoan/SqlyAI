using SqlyAI.Domain.Db;
using System.ComponentModel.DataAnnotations;

namespace SqlyAI.Application.Db.Dtos
{
    public class ReportConfigurationInput
    {
        public Guid QueryId { get; set; }

        [Required]
        public ReportType ReportType { get; set; }
        [Required]
        public string XAxisColumn { get; set; }
        [Required]
        public CalculationMethod XAxisCalculationMethod { get; set; }
        [Required]
        public string YAxisColumn { get; set; }
        [Required]
        public CalculationMethod YAxisCalculationMethod { get; set; }
    }
}