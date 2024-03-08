//using System.ComponentModel.DataAnnotations.Schema;
//using Abp.Domain.Entities.Auditing;
//using Abp.Domain.Entities;
//using System.Collections.Generic;
//using Abp.Organizations;

//namespace App.Db
//{
//    [Table("Stories")]
//    public class Story : FullAuditedEntity<long>, IMayHaveTenant, IMayHaveOrganizationUnit
//    {
//        public int? TenantId { get; set; }
//        public virtual long? OrganizationUnitId { get; set; }

//        [ForeignKey("OrganizationUnitId")]
//        public OrganizationUnit OrganizationUnit { get; set; }

//        public virtual string Name { get; set; }

//        public virtual string Description { get; set; }

//        public virtual ICollection<Query> Queries { get; set; } = new List<Query>();

//        public List<string> QueryTables { get; set; }

//        public virtual int? DatabaseId { get; set; }

//        [ForeignKey("DatabaseId")]
//        public Database Database { get; set; }
//    }
//}