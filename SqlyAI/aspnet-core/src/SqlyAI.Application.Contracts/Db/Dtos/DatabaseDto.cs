namespace SqlyAI.Application.Dtos
{
    public class DatabaseDto : EntityDto<Guid>
    {
        public DbType Type { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public virtual Guid? OrganizationUnitId { get; set; }


    }
}