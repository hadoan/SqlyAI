namespace SqlyAI.Application.Db.Dtos
{
    public class StoryDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? DatabaseId { get; set; }

        public virtual Guid? OrganizationUnitId { get; set; }

    }
}