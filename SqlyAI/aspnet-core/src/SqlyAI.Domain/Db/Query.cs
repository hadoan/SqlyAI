using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace SqlyAI.Domain.Db
{

    [Table("Queries")]
    public class Query : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Text { get; set; }

        [Required]
        public virtual string Sql { get; set; }

        public virtual long? MetaDataId { get; set; }

        [ForeignKey("MetaDataId")]
        public virtual QueryMetaData MetaData { get; set; }

        public virtual Guid? DatabaseId { get; set; }

        [ForeignKey("DatabaseId")]
        public Database Database { get; set; }

        public List<string> QueryTables { get; set; }

        public virtual Guid? OrganizationUnitId { get; set; }

        [ForeignKey("OrganizationUnitId")]
        public OrganizationUnit OrganizationUnit { get; set; }

        public Guid? StoryId { get; set; }
        [ForeignKey("StoryId")]
        public Story Story { get; set; }

    }


}