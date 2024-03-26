namespace SqlyAI.Application.Db.Dtos
{
    public class GetStoryForEditOutput
    {
        public CreateOrEditStoryDto Story { get; set; }

        public string DatabaseName { get; set; }

        public string OrganizationUnitName { get; set; }

    }
}