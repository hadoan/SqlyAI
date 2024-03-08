
using App.Integrations.Dtos;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using DbType = App.Db.DbType;

namespace SqlyAI.Databases
{
    public class DatabaseQueryService : BaseDatabaseService, IDatabaseQueryService
    {
        public async Task<IEnumerable<dynamic>> RunSqlQuery(string sql, DbType type, string connectionString)
        {
            using var connection = GetConnection(type, connectionString);
            return await connection.QueryAsync(sql);
        }

        public async Task<int> ExecuteSqlQuery(string sql, DbType type, string connectionString)
        {
            using var connection = GetConnection(type, connectionString);
            Console.WriteLine("Executing...");
            Console.WriteLine(sql);
            return await connection.ExecuteAsync(sql);
        }

        public async Task RunSqlBulkCopyData(string tableName, DynamicTable table, string connectionString)
        {
            using var bulkCopy = new SqlBulkCopy(connectionString);
            // Set the timeout.
            bulkCopy.BulkCopyTimeout = 10 * 60; //10 minute
            bulkCopy.DestinationTableName = tableName;

            // Define the column mappings
            foreach (var columnName in table.ColumnNames)
            {
                bulkCopy.ColumnMappings.Add(columnName.ColumnName, columnName.ColumnName);
            }
            // Convert the List<T> to a DataTable
            DataTable dataTable = new DataTable();
            dataTable = ToDataTable(table, tableName);

            // Insert the data into the SQL Server table
            bulkCopy.WriteToServer(dataTable);
        }

        private DataTable ToDataTable(DynamicTable table, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);

            //Get all the properties
            foreach (var columnInfo in table.ColumnNames)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(columnInfo.ColumnName);
            }
            foreach (var item in table.Rows)
            {
                var values = new object[item.Values.Count];
                for (int i = 0; i < item.Values.Count; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = item.Values[i].RawValue;
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }
}
