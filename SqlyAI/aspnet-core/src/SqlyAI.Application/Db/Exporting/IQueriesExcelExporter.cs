using Pillio.Dto;

namespace SqlyAI.Application.Db.Exporting
{
    public interface IQueriesExcelExporter
    {
        FileDto ExportToFile(List<GetQueryForViewDto> queries);
    }
}