

namespace SqlyAI.Application.Dtos
{
    public class GetQueryForViewDto
    {
        public QueryDto Query { get; set; }

        public string DatabaseName { get; set; }

        public string OrganizationUnitName { get; set; }

    }
}