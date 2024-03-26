namespace SqlyAI.Application.Dtos
{
    public class GetDatabaseForEditOutput
    {
        public CreateOrEditDatabaseDto Database { get; set; }
        public string OrganizationUnitName { get; set; }
    }
}