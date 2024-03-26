namespace SqlyAI.Application.Dtos
{
    public class CreateOrEditStoryDto : EntityDto<Guid?>
    {
        public string Name { get; set; }


        public string Description { get; set; }

        public List<CreateOrEditQueryDto> Queries { get; set; } = new List<CreateOrEditQueryDto>();


        public Guid? DatabaseId { get; set; }

        public List<string> QueryTables { get; set; }

        public virtual long? OrganizationUnitId { get; set; }
    }
}