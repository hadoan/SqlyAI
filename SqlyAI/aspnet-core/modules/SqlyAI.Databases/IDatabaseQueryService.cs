using App.Db;
using App.Integrations.Dtos;

namespace SqlyAI.Databases
{
    public interface IDatabaseQueryService
    {
        Task<IEnumerable<dynamic>> RunSqlQuery(string sql, DbType type, string connectionString);
        Task<int> ExecuteSqlQuery(string sql, DbType type, string connectionString);

        Task RunSqlBulkCopyData(string tableName, DynamicTable table, string connectionString);
    }
}