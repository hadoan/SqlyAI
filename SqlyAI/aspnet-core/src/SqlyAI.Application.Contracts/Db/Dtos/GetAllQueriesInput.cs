namespace SqlyAI.Application.Db.Dtos
{
    public class GetAllQueriesInput : PagedAndSortedResultRequestDto
    {
        public string NameFilter { get; set; }

        public string Filter { get; set; }

        public string TextFilter { get; set; }

        public string SqlFilter { get; set; }


        public string DatabaseNameFilter { get; set; }

        public DbType? DbType { get; set; }

    }
}