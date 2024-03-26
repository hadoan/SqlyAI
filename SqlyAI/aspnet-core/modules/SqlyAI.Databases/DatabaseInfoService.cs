using SqlyAI.Databases.Dtos;
using Dapper;
using AppCommon.Enums;

namespace SqlyAI.Databases
{
    public class DatabaseInfoService : BaseDatabaseService, IDatabaseInfoService
    {
        public Task<IEnumerable<string>> GetTableNames(DbType type, string connStr)
        {
            using var connection = GetConnection(type, connStr);
            if (connection != null && !string.IsNullOrEmpty(connection.Database))
            {
                var dbName = connection.Database;
                var tableNameQuery = GetTableNamesQuery(type, dbName);
                var tableNames = connection.QueryAsync<string>(tableNameQuery);
                return tableNames;
            }
            return Task.FromResult(new List<string>().AsEnumerable());
        }

        public async Task<IEnumerable<DbSchemaInfo>> GetDbSchemaInfos(DbType type, string connStr, string csvTable = null, string csvDbName = null)
        {
            var connectionType = (type == DbType.CSV || type == DbType.GoogleSheet) ? DbType.SQLServer : type;
            using var connection = GetConnection(connectionType, connStr);
            if (connection != null && !string.IsNullOrEmpty(connection.Database))
            {
                var dbName = connection.Database;
                var query = GetTableAndColumnNamesQuery(type, dbName, csvTable, csvDbName);
                Console.WriteLine(query);
                var data = await connection.QueryAsync<DbSchemaInfo>(query);
                return data;
            }
            return new List<DbSchemaInfo>().AsEnumerable();
        }

        public string GetSqlColumnType(string columnType)
        {
            //var type = ColumnType.ParseName(columnType);
            switch (columnType)
            {
                case nameof(ColumnType.JSON):
                    return "nvarchar(max)";
                case nameof(ColumnType.NUMBER):
                    return "decimal(18,2)";
                case nameof(ColumnType.DATE):
                    return "datetime2(7)";
                case nameof(ColumnType.STRING):
                    return "nvarchar(max)";
                default:
                    return "nvarchar(max)";

            }
        }
    }
}
