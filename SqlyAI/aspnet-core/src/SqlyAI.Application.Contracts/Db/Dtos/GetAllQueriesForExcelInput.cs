namespace SqlyAI.Application.Db.Dtos
{
    public class GetAllQueriesForExcelInput
    {
        public string Filter { get; set; }

        public string TextFilter { get; set; }

        public string SqlFilter { get; set; }

        public string MetaDataFilter { get; set; }

    }
}