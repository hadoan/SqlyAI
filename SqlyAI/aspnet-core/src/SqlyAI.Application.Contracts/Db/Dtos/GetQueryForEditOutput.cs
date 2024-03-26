namespace SqlyAI.Application.Db.Dtos
{
    public class GetQueryForEditOutput
    {
        public CreateOrEditQueryDto Query { get; set; }

        public string DatabaseName { get; set; }

        public string OrganizationUnitName { get; set; }

        public QueryMetaDataDto MetaData { get; set; }
    }
}