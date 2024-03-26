using AppCommon;
using AppCommon.Enums;
using Humanizer;
using SqlyAI.Databases;
using SqlyAI.Databases.Dtos;
using SqlyAI.Importers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace SqlyAI.Domain.Db
{
    public class DatabaseManager : DomainService, ITransientDependency
    {
        private readonly IRepository<Database, Guid> _databaseRepository;
        private readonly IDatabaseInfoService _databaseInfoService;
        private readonly IRepository<CsvImporter> _csvImporterRepository;
        private readonly string csvDbConnectionString;
        public DatabaseManager(IRepository<Database, Guid> databaseRepository, IDatabaseInfoService databaseInfoService, IAppIntegrationConfigurationAccessor configuration, IRepository<CsvImporter> csvImporterRepository)
        {
            _databaseRepository = databaseRepository;
            _databaseInfoService = databaseInfoService;
            csvDbConnectionString = configuration.Configuration["ConnectionStrings:CsvDb"];
            _csvImporterRepository = csvImporterRepository;
        }

        public string? GetCsvTableName(Guid tenantId, Guid fileId, string sqlTableName)
        {
            var name = $"table_{tenantId}_{fileId}_{sqlTableName}";
            return name.Underscore();
        }

        public async Task<IEnumerable<DbSchemaInfo>> GetDbSchemaInfoAsync(Database db)
        {
            var connectionString = db.Type == DbType.CSV || db.Type == DbType.GoogleSheet ? csvDbConnectionString : db.ConnectionString;
            string csvTable = null;
            string csvDbName = null;
            if (db.Type == DbType.CSV || db.Type == DbType.GoogleSheet)
            {
                var fileId = Guid.Parse(db.ConnectionString);
                var csvImporter = await _csvImporterRepository.GetAsync(x => x.FileId == fileId);
                csvTable = csvImporter?.TableName;
                csvDbName = csvImporter?.FileName;
            }
            var infoes = await _databaseInfoService.GetDbSchemaInfos(db.Type, connectionString, csvTable, csvDbName);
            return infoes;
        }

        public string? GetCsvConnString()
        {
            return csvDbConnectionString;
        }
    }
}
