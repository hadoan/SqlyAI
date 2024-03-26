using System.ComponentModel.DataAnnotations;

namespace SqlyAI.Application.Dtos
{
    public class CreateOrEditQueryDto : EntityDto<Guid?>
    {
        public string Name { get; set; }


        public string Text { get; set; }

        [Required]
        public string Sql { get; set; }

        public Guid? DatabaseId { get; set; }

        public List<string> QueryTables { get; set; }

        public virtual Guid? OrganizationUnitId { get; set; }

        public DbType? DbType { get; set; }

        public Guid? StoryId { get; set; }
    }
}