using AppCommon.Dtos;
using SqlyAI.Importers;

namespace SqlyAI.Application.Db.Dtos
{
    public class ImportCsvInput
    {
        public List<ColumnInfo> ColumnNames { get; set; }
        public Guid FileId { get; set; }

        public SourceType? SourceType
        {
            get; set;
        }
    }
}
