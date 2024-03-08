//using App.Db;
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using Abp.Domain.Entities.Auditing;
//using Abp.Domain.Entities;
//using System.Collections.Generic;
//using Abp.Organizations;

//namespace App.Db
//{

//    [Table("Queries")]
//    public class Query : FullAuditedEntity<long>, IMayHaveTenant, IMayHaveOrganizationUnit
//    {
//        public int? TenantId { get; set; }
        
//        public virtual string Name { get; set; }
        
//        public virtual string Text { get; set; }

//        [Required]
//        public virtual string Sql { get; set; }

//        public virtual long? MetaDataId { get; set; }

//        [ForeignKey("MetaDataId")]
//        public virtual QueryMetaData MetaData { get; set; }

//        public virtual int? DatabaseId { get; set; }

//        [ForeignKey("DatabaseId")]
//        public Database Database { get; set; }

//        public List<string> QueryTables { get; set; }

//        public virtual long? OrganizationUnitId { get; set; }

//        [ForeignKey("OrganizationUnitId")]
//        public OrganizationUnit OrganizationUnit { get; set; }

//        public long? StoryId { get; set; }
//        [ForeignKey("StoryId")]
//        public Story Story { get; set; }    

//    }

   
//}