namespace SqlyAI.Application.Db.Dtos
{
    public class QueryDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public string Sql { get; set; }

        public QueryMetaDataDto MetaData { get; set; }

        public Guid? DatabaseId { get; set; }

        public List<string> QueryTables { get; set; }

        public virtual Guid? OrganizationUnitId { get; set; }

    }
}