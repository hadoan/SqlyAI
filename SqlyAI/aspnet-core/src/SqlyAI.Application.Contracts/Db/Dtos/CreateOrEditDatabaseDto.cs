using System.ComponentModel.DataAnnotations;

namespace SqlyAI.Application.Dtos
{
    public class CreateOrEditDatabaseDto : EntityDto<Guid?>
    {

        public DbType Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }

        public long? OrganizationUnitId { get; set; }

    }
}