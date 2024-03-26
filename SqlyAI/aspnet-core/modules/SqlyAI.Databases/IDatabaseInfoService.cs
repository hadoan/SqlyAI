using SqlyAI.Databases.Dtos;
using AppCommon.Enums;

namespace SqlyAI.Databases
{
    public interface IDatabaseInfoService
    {
        Task<IEnumerable<DbSchemaInfo>> GetDbSchemaInfos(DbType type, string connStr, string csvTable = null, string csvDbName = null);
        Task<IEnumerable<string>> GetTableNames(DbType type, string connStr);

        string GetSqlColumnType(string columnType);
    }
}