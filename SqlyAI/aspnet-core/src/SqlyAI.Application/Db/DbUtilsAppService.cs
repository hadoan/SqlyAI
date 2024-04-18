//using CsvHelper;
//using System.Globalization;
//using System.IO;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using SqlyAI.Databases;
//using TSQL.Statements;
//using TSQL;
//using Volo.Abp.BackgroundJobs;
//using Pillio.BackgrounJobs;
//using Microsoft.Extensions.Logging;
//using Pillio.Dto;

//namespace SqlyAI.Application.Db
//{

//    public class DbUtilsAppService : SqlyAIAppService
//    {
//        private readonly IOpenAIService _openAiService;
//        private readonly IDatabaseInfoService _dbInfoService;
//        private readonly IDatabaseQueryService _databaseQueryService;
//        private readonly IRepository<Database, Guid> _databaseRepository;
//        private readonly IRepository<Query, Guid> _queryRepository;
//        //private readonly ITempFileCacheManager _tempFileCacheManager;
//        private readonly IBackgroundJobManager _jobManager;
//        private readonly IRepository<QueryMetaData, Guid> _metaDataRepository;
//        private readonly DatabaseManager _databaseManager;
//        private readonly IRepository<CsvImporter, Guid> _csvImporterRepository;

//        public DbUtilsAppService(IOpenAIService openAiService, IDatabaseQueryService databaseQueryService, IRepository<Database, Guid> databaseRepository, IDatabaseInfoService dbInfoService, IRepository<Query, Guid> queryRepository, IBackgroundJobManager jobManager, IRepository<QueryMetaData, Guid> metaDataRepository, DatabaseManager databaseManager, IRepository<CsvImporter, Guid> csvImporterRepository)
//        {
//            _openAiService = openAiService;
//            _databaseQueryService = databaseQueryService;
//            _databaseRepository = databaseRepository;
//            _dbInfoService = dbInfoService;
//            _queryRepository = queryRepository;
//            _jobManager = jobManager;
//            _metaDataRepository = metaDataRepository;
//            _databaseManager = databaseManager;
//            _csvImporterRepository = csvImporterRepository;
//        }

//        public async Task<string> TextToSql(TextToSqlInput input)
//        {
//            var db = await _databaseRepository.GetAsync(input.DbId);
//            var tables = input.Tables;
//            if (db.Type == DbType.CSV || db.Type == DbType.GoogleSheet)
//            {
//                tables = new List<string> { db.Name };
//            }
//            var schemas = await _databaseManager.GetDbSchemaInfoAsync(db);
//            var selectedTableSchemas = schemas.Where(x => tables.Contains(x.TableName))
//                .GroupBy(input => input.TableName)
//                .Select(x => new TableInfo(x.Key, x.Select(x => x.ColumnName).ToArray()));
//            var completionOutput = await _openAiService.TextToSql(db.Type, input.Text, selectedTableSchemas);
//            if (completionOutput.Error != null)
//            {
//                throw new UserFriendlyException(completionOutput.Error.message);
//            }
//            return completionOutput.Choices.Count > 0 ? completionOutput.Choices[0].Text : "";
//        }

//        public async Task<DynamicSqlOutput> RunQuery(Guid queryId, bool max100 = true)
//        {
//            var query = await _queryRepository.GetAsync(queryId);
//            return await RunSql(queryId, query.Sql, query.DatabaseId ?? Guid.Empty,max100);
//        }
//        private async Task<DynamicSqlOutput> RunSql(Guid queryId, string sql, Guid dbId, bool max100 = true)
//        {
//            try
//            {
//                var meta = await _metaDataRepository.FindAsync(x => x.QueryId == queryId);
//                sql = sql.Trim().Trim(Environment.NewLine.ToArray());
//                sql = sql.Replace("  ", " ");

//                var db = await _databaseRepository.GetAsync(dbId);
//                if (max100 && sql.StartsWith("select", true, null))
//                {
//                    switch (db.Type)
//                    {
//                        case DbType.SQLServer:
//                        case DbType.CSV:
//                        case DbType.GoogleSheet:
//                            {
//                                sql = sql.StartsWith("select top", StringComparison.InvariantCultureIgnoreCase) ? sql : "select top 100" + sql.Substring("select".Length);
//                                break;
//                            }
//                        case DbType.MySql:
//                        case DbType.PostgreSQL:
//                            {
//                                sql = sql.Contains("limit", StringComparison.InvariantCultureIgnoreCase) ? sql : sql.TrimEnd(',') + " limit 100";
//                                break;
//                            }
//                        default:
//                            break;
//                    }
//                }
//                var connStr = db.ConnectionString;
//                if (db.Type == DbType.CSV || db.Type == DbType.GoogleSheet)
//                {
//                    var fileId = Guid.Parse(db.ConnectionString);
//                    var importer = await _csvImporterRepository.FindAsync(x => x.FileId == fileId);

//                    //if (sql.Contains(".csv"))
//                    //{
//                    //    sql = sql.Replace(db.Name, importer.TableName);
//                    //}
//                    //else
//                    //{
//                    //    sql = sql.Replace(db.Name.TrimEnd(".csv".ToArray()), importer.TableName);
//                    //}
//                    TSQLSelectStatement select = TSQLStatementReader.ParseStatements(sql)[0] as TSQLSelectStatement;
//                    var fromTable = sql.Substring(select.From.BeginPosition).Replace("FROM", "").Replace("from", "").Trim();
//                    if (!string.IsNullOrEmpty(fromTable))
//                    {
//                        var nameWithCsv = fromTable + ".csv";
//                        var dbTableName = $"[{importer.TableName}]";
//                        if (sql.Contains(nameWithCsv))
//                            sql = sql.Replace(nameWithCsv, dbTableName);
//                        else sql = sql.Replace(fromTable, dbTableName);
//                    }
//                    connStr = _databaseManager.GetCsvConnString();
//                }

//                IEnumerable<dynamic> records = await _databaseQueryService.RunSqlQuery(sql, db.Type, connStr);
//                var headers = new List<string>();
//                var rows = new List<DynamicSqlRowItem>();
//                if (records.Any())
//                {
//                    var propertyNames = records.First();
//                    foreach (var propertyName in propertyNames)
//                    {

//                        headers.Add(propertyName.Key);
//                    }
//                    foreach (var record in records)
//                    {
//                        var row = new List<KeyValuePair<string, object>>();
//                        foreach (var property in record)
//                        {
//                            var key = property.Key.ToString();
//                            var value = property.Value;
//                            row.Add(new KeyValuePair<string, object>(key, value));//new column
//                            if (value != null && meta != null && meta.ColumnConfigs.Any(x => x.ColumnName == key && x.ColumnType == ColumnType.JSON))
//                            {
//                                ParseJsonObjectProperties(headers, row, key, value);
//                            }
//                        }
//                        rows.Add(new DynamicSqlRowItem(Guid.NewGuid(), row));
//                    }
//                }

//                //await _jobManager.EnqueueAsync<UpdateQueryLastQueryColumnsJob>(new (queryId, headers));
//                await _jobManager.EnqueueAsync<UpdateQueryLastQueryColumnsJobArgs>(new UpdateQueryLastQueryColumnsJobArgs()
//                {
//                    QueryId = queryId,
//                    Headers = headers,
//                });

//                return new DynamicSqlOutput(headers, rows);
//            }
//            catch (Exception ex)
//            {
//                Logger.LogError("RunSql", ex);
//                throw new UserFriendlyException(ex.Message);
//            }
//        }

//        private void ParseJsonObjectProperties(List<string> headers, List<KeyValuePair<string, object>> row, dynamic key, dynamic value)
//        {

//            try
//            {
//                JObject jsonObject = JObject.Parse(value.ToString());
//                foreach (var jsonProperty in jsonObject.Properties())
//                {
//                    // Console.WriteLine(property.Name + ": " + property.Value);
//                    var columnName = $"${key}.{jsonProperty.Name}";
//                    if (jsonProperty.Value.Type == JTokenType.Object)
//                    {
//                        ParseJsonObjectProperties(headers, row, columnName, jsonProperty.Value);
//                    }
//                    else
//                    {
//                        if (headers.All(x => x != columnName))
//                        {
//                            headers.Add(columnName);
//                        }
//                        var jsonValue = jsonProperty.Value.Type == JTokenType.Array ? JsonConvert.SerializeObject(jsonProperty.Value) : jsonProperty.Value;
//                        row.Add(new KeyValuePair<string, object>(columnName, jsonValue));//new column
//                    }
//                }
//            }
//            catch (JsonReaderException)
//            {
//                Console.WriteLine("The JSON is not valid.");
//            }
//        }

//        public async Task<FileDto> DownloadQueryCsv(Guid queryId)
//        {
//            try
//            {

//                var query = await _queryRepository.GetAsync(queryId);
//                var sqlResult = await this.RunSql(queryId, query.Sql, query.DatabaseId ?? Guid.Empty, false);
//                using var stream = new MemoryStream();
//                using var writer = new StreamWriter(stream);
//                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

//                foreach (var column in sqlResult.Columns)
//                {
//                    csv.WriteField(column);
//                }
//                csv.NextRecord();
//                foreach (var row in sqlResult.Records)
//                {
//                    foreach (var column in sqlResult.Columns)
//                    {
//                        var hasValue = row.Columns.Any(x => x.Key == column);
//                        if (hasValue)
//                        {
//                            csv.WriteField(row.Columns.First(x => x.Key == column).Value?.ToString());
//                        }
//                        else
//                        {
//                            csv.WriteField("");
//                        }
//                    }
//                    csv.NextRecord();
//                }

//                var file = new FileDto($"query-{query.Name}.csv", "text/csv");
//                throw new NotImplementedException();
//                //_tempFileCacheManager.SetFile(file.FileToken, stream.ToArray());
//                return file;

//            }
//            catch (Exception ex)
//            {
//                Logger.LogError("ExportCsv", ex);
//                throw new UserFriendlyException(ex.Message);
//            }
//        }
//    }
//    public record TextToSqlInput(string Text, Guid DbId, List<string> Tables);
//    public record DynamicSqlRowItem(Guid RowId, List<KeyValuePair<string, object>> Columns);
//    public record DynamicSqlOutput(List<string> Columns, List<DynamicSqlRowItem> Records);
//}
