namespace SqlyAI.Application.Dtos
{
    public class GetDatabaseForViewDto
    {
        public DatabaseDto Database { get; set; }
        public string OrganizationUnitName { get; set; }

        public string CsvFileId { get; set; }
    }
}