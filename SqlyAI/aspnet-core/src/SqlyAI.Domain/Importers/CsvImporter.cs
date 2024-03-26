using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Domain.Entities.Auditing;
using AppCommon.Dtos;

namespace SqlyAI.Importers
{
    [Table("CsvImporters")]
    public class CsvImporter : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        [Range(1, 2)]
        public string Delimiter { get; set; }

        private List<ColumnInfo> columnNames = new List<ColumnInfo>();

        public Guid? TenantId { get; set; }

        [Required]
        public virtual string FileName { get; set; }

        public virtual Guid FileId { get; set; }

        public virtual string? FileType { get; set; }

        public virtual double FileSize { get; set; }

        public virtual string? ExtraInfo { get; set; }

        public string TableName { get; set; }

        public List<ColumnInfo> ColumnNames { get => columnNames ?? new List<ColumnInfo>(); set => columnNames = value; }

        public SourceType? SoureType { get; set; }

        public string? Source { get; set; }
    }
}