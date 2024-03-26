namespace SqlyAI.Application.Dtos
{
    public class GetAllDatabasesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? TypeFilter { get; set; }

        public string NameFilter { get; set; }

        public string ConnectionStringFilter { get; set; }

    }
}