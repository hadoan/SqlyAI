namespace SqlyAI.Application.Dtos
{
    public class QueryDatabaseLookupTableDto
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public DbType Type { get; set; }
    }
}