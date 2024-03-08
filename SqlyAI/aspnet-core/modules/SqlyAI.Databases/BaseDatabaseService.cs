using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;
using DbType = App.Db.DbType;

namespace SqlyAI.Databases
{
    public abstract class BaseDatabaseService
    {
        protected IDbConnection GetConnection(DbType type, string connStr)
        {
            IDbConnection connection = null;
            switch (type)
            {
                case DbType.MySql:
                    connection = new MySqlConnection(connStr);
                    break;
                case DbType.SQLServer:
                case DbType.CSV:
                case DbType.GoogleSheet:
                    connection = new SqlConnection(connStr);
                    break;
                case DbType.PostgreSQL:
                    connection = new NpgsqlConnection(connStr);
                    break;
                default:
                    break;
            }
            return connection;
        }


        protected string GetTableAndColumnNamesQuery(DbType type, string dbName, string csvTable = null,string csvDbName = null)
        {
            var query = "";
            switch (type)
            {
                case DbType.MySql:
                    query = $"""
                        SELECT TABLE_NAME as TableName, COLUMN_NAME as ColumnName, DATA_TYPE as DataType
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_SCHEMA = '{dbName}'
                        """;
                    break;
                case DbType.SQLServer:
                    query = """
                        SELECT TABLE_NAME as TableName, COLUMN_NAME as ColumnName, DATA_TYPE as DataType
                        FROM INFORMATION_SCHEMA.COLUMNS
                        """;
                    break;
                case DbType.PostgreSQL:
                    query = """
                        SELECT table_name as TableName, column_name as ColumnName, data_type as DataType
                        FROM information_schema.columns
                        WHERE table_schema='public';
                        """;
                    break;
                case DbType.CSV:
                case DbType.GoogleSheet:
                    query = $"""
                        SELECT '{csvDbName}' as TableName, COLUMN_NAME as ColumnName, DATA_TYPE as DataType
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = '{csvTable}'
                        """;
                    break;
                default:
                    break;
            }
            return query;
        }


        protected string GetTableNamesQuery(DbType type, string dbName)
        {
            var tableNameQuery = "";
            switch (type)
            {
                case DbType.MySql:
                    tableNameQuery = $"show tables from `{dbName}`";
                    break;
                case DbType.SQLServer:
                    tableNameQuery = "SELECT name FROM sys.Tables";
                    break;
                case DbType.PostgreSQL:
                    tableNameQuery = """
                        SELECT table_name
                         FROM information_schema.tables
                        WHERE table_schema='public'
                          AND table_type='BASE TABLE';
                        """;
                    break;
                default:
                    break;
            }
            return tableNameQuery;
        }

        protected string GetColumnNamesQuery(DbType type, string tableName)
        {
            var tableNameQuery = "";
            switch (type)
            {
                case DbType.MySql:
                    tableNameQuery = $"""
                        SELECT COLUMN_NAME as ColumnName , DATA_TYPE as DataType
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = '{tableName}'
                        """;
                    break;
                case DbType.SQLServer:
                    tableNameQuery = $"""
                        SELECT COLUMN_NAME as ColumnName, DATA_TYPE as DataType
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_NAME = N'{tableName}'
                        """;
                    break;
                case DbType.PostgreSQL:
                    tableNameQuery = """
                        SELECT table_name
                         FROM information_schema.tables
                        WHERE table_schema='public'
                          AND table_type='BASE TABLE';
                        """;
                    break;
                default:
                    break;
            }
            return tableNameQuery;
        }
    }
}
