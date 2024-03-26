using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Identity;
using Volo.Abp.Domain.Entities.Auditing;

namespace SqlyAI.Domain.Db
{
    [Table("Stories")]
    public class Story : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        protected Story() { }
        public Story(Guid id)
        {
            this.Id = id;
        }
        public Guid? TenantId { get; set; }
        public virtual long? OrganizationUnitId { get; set; }

        [ForeignKey("OrganizationUnitId")]
        public OrganizationUnit OrganizationUnit { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<Query> Queries { get; set; } = new List<Query>();

        public List<string> QueryTables { get; set; }

        public virtual Guid? DatabaseId { get; set; }

        [ForeignKey("DatabaseId")]
        public Database Database { get; set; }
    }
}