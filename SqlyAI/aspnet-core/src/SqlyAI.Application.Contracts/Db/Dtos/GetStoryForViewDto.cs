namespace SqlyAI.Application.Dtos
{
    public class GetStoryForViewDto
    {
        public StoryDto Story { get; set; }

        public string DatabaseName { get; set; }

        public string OrganizationUnitName { get; set; }
    }
}