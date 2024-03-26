using AppCommon.Dtos;

namespace SqlyAI.Integrations.Interfaces
{
    public interface ICsvService
    {
        Task<DynamicTable> ParseCsvFile(Stream stream, string delimiter = ";");

        Task<int> GenerateSqlTable(string tableName, List<ColumnInfo> columnNames);
        Task ImportCsvData(string tableName, DynamicTable table);
        Task ReImportCsvData(string tableName, DynamicTable table);

    }
}
