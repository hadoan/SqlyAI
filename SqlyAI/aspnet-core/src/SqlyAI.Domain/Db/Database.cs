using AppCommon.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
namespace SqlyAI.Domain.Db
{
    [Table("Databases")]
    public class Database : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        protected Database() { }
        public Database(Guid id):base(id)
        {
            
        }
        public Guid? TenantId { get; set; }

        public virtual DbType Type { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string ConnectionString { get; set; }

        [ForeignKey("OrganizationUnitId")]
        public OrganizationUnit? OrganizationUnit { get; set; }

        public virtual Guid? OrganizationUnitId { get; set; }
    }
}