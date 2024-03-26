namespace SqlyAI.Application.Dtos
{
    public class GetAllStoriesInput : PagedAndSortedResultRequestDto
    {
        public string NameFilter { get; set; }

        public string Filter { get; set; }


        public string DatabaseNameFilter { get; set; }

        public DbType? DbType {get;set;}
    }
}