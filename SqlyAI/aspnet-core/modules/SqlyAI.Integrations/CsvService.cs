using SqlyAI.Integrations.Interfaces;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;
using System.Text.RegularExpressions;
using SqlyAI.Databases;
using AppCommon;
using AppCommon.Dtos;
using AppCommon.Enums;

namespace SqlyAI.Integrations
{
    public class CsvService : ICsvService
    {
        public static string FILE_CACHE_NAME = "ImportedCSVFiles";

        private readonly IDatabaseQueryService _databaseQueryService;
        private readonly IDatabaseInfoService _databaseInfoService;
        private readonly string csvDbConnectionString;

        public CsvService(IAppIntegrationConfigurationAccessor configuration, IDatabaseQueryService databaseQueryService, IDatabaseInfoService databaseInfoService)
        {
            _databaseQueryService = databaseQueryService;
            csvDbConnectionString = configuration.Configuration["ConnectionStrings:CsvDb"];
            _databaseInfoService = databaseInfoService;
        }



        public async Task<int> GenerateSqlTable(string tableName, List<ColumnInfo> columnNames)
        {
            //generate sql server create table with column in columnNames
            var columnQuery = string.Join(",", columnNames.Select(x => $"[{x.ColumnName}] {x.Type.ToString()}"));
            string queryString = $"CREATE TABLE {tableName} ({columnQuery})";

            return await _databaseQueryService.ExecuteSqlQuery(queryString, DbType.SQLServer, csvDbConnectionString);
        }

        public Task ImportCsvData(string tableName, DynamicTable table)
        {
            return _databaseQueryService.RunSqlBulkCopyData(tableName, table, csvDbConnectionString);
        }
        public async Task ReImportCsvData(string tableName, DynamicTable table)
        {
            var deleteCommand = $"DELETE FROM {tableName}";
            await _databaseQueryService.ExecuteSqlQuery(deleteCommand, DbType.SQLServer, csvDbConnectionString);
            await _databaseQueryService.RunSqlBulkCopyData(tableName, table, csvDbConnectionString);
        }

        public async Task<DynamicTable> ParseCsvFile(Stream stream, string delimiter = ";")
        {
            var table = new DynamicTable();
            using var reader = new StreamReader(stream);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = null,
                HasHeaderRecord = true,
                MissingFieldFound = null,
                Delimiter = delimiter,
            };
            using var csv = new CsvReader(reader, config);
            csv.Read();
            csv.ReadHeader();
            var headers = csv.Context.Reader.HeaderRecord;
            if (headers == null || headers.Count() == 0) { return table; }
            int i = 0;
            var emptyColumnNames = new List<string>();
            foreach (var header in headers)
            {
                var columnHeader = !string.IsNullOrEmpty(header) ? header : "column_" + Guid.NewGuid().ToString();
              
                if (table.ColumnNames.Any(x => x.RawName == columnHeader))
                {
                    columnHeader = header + "_" + i;
                }
                string columnName = Regex.Replace(columnHeader.Replace(" ", "_"), "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
                columnName = columnName.ToLower();

                if (string.IsNullOrEmpty(header)) emptyColumnNames.Add(columnName);
                var columnInfo = new ColumnInfo
                {
                    RawName = header,
                    ColumnName = columnName,
                    Order = i + 1,
                    Type = ColumnType.STRING,
                };
                table.ColumnNames.Add(columnInfo);
                i++;
            }
            var recods = csv.GetRecords<dynamic>().ToList();

            int rowIndex = 0;
            foreach (var record in recods)
            {
                var row = new RowInfo<int> { RowId = rowIndex + 1 };
                i = 0;
                var blankColumnNameIndex = 0;
                foreach (var column in record)
                {
                    var columns = table.ColumnNames.Where(x => x.RawName == column.Key).ToList();
                    if (string.IsNullOrEmpty(column.Key))
                    {
                        columns = table.ColumnNames.Where(x => x.ColumnName == emptyColumnNames[blankColumnNameIndex]).ToList();
                        blankColumnNameIndex += 1;
                    }
                    var columnName = columns?.FirstOrDefault()?.ColumnName;
                    if (columnName != null)
                    {
                        if (columns.Count() >= 2)
                        {
                            columnName = columns.Where(x => x.ColumnName.EndsWith("_" + i)).FirstOrDefault()?.ColumnName ?? columnName;
                        }

                    }
                    else
                    {
                        columnName = "undefined_" + i;
                    }
                    var columnValue = new ColumnValue { ColumnName = columnName, RawValue = column.Value };
                    row.Values.Add(columnValue);
                    i++;
                }
                row.RawValue = string.Join(delimiter, row.Values.Select(x => x.RawValue).ToArray());
                table.Rows.Add(row);

                rowIndex++;
            }
            return table;
        }

    }
}
