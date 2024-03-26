

namespace SqlyAI.Application.Db
{
    public class CsvImporterJobAppService : SqlyAIAppService
    {
        private readonly IStringEncryptionService _stringEncryptionService;

        private readonly IDistributedCache<DynamicTable, Guid> _cacheManager;
        private readonly ICsvService _csvService;
        private readonly IRepository<CsvImporter, Guid> _csvImporterRepository;
        private readonly IRepository<Database, Guid> _databaseRepository;
        private readonly DatabaseManager _databaseManager;
        private IUnitOfWorkManager _unitOfWorkManager;
        private readonly IGuidGenerator _guidGenerator;
        public CsvImporterJobAppService(IDistributedCache<DynamicTable, Guid> cacheManager, ICsvService csvService, IRepository<CsvImporter, Guid> csvImporterRepository, IRepository<Database, Guid> databaseRepository, DatabaseManager databaseManager, IUnitOfWorkManager unitOfWorkManager,  IGuidGenerator guidGenerator, IStringEncryptionService stringEncryptionService)
        {
            _cacheManager = cacheManager;
            _csvService = csvService;
            _csvImporterRepository = csvImporterRepository;
            _databaseRepository = databaseRepository;
            _databaseManager = databaseManager;
            _unitOfWorkManager = unitOfWorkManager;
            _guidGenerator = guidGenerator;
            _stringEncryptionService = stringEncryptionService;
        }


        public async Task<Guid?> ImportCsv(ImportCsvInput input)
        {
            Guid? dbId = null;
            var data = await _cacheManager.GetAsync(input.FileId);
                   
            if (data == null)
            {
                throw new UserFriendlyException($"Couldn't find uploaded CSV file with id {input.FileId}");
            }
            var tenantId = CurrentTenant.Id;
            var existing = await _csvImporterRepository.FindAsync(x => x.FileId == input.FileId);
            if (existing == null)
            {
                var tableName = _databaseManager.GetCsvTableName(tenantId ?? Guid.Empty, data.FileId, data.SqlTableName);
                var importer = new CsvImporter
                {
                    TenantId = tenantId,
                    FileName = data.FileName,
                    FileId = data.FileId,
                    FileType = data.FileType,
                    FileSize = data.FileSize,
                    TableName = tableName,
                    ColumnNames = data.ColumnNames,
                    Delimiter = data.Delimiter,
                    SoureType = input.SourceType,
                    Source = data.FileUrl
                };

                var db = new Database(_guidGenerator.Create())
                {
                    TenantId = tenantId,
                    Type = input.SourceType.HasValue && input.SourceType.Value == SourceType.GoogleSheet ? DbType.GoogleSheet : DbType.CSV,
                    Name = data.FileName,
                    ConnectionString = data.FileId.ToString(),
                };
                using var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions
                {
                    IsTransactional = false
                });

                await _databaseRepository.InsertAsync(db);
                await _csvImporterRepository.InsertAsync(importer);
                await _csvService.GenerateSqlTable(tableName, input.ColumnNames);
                await uow.CompleteAsync();
                using var newUow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions
                {
                    IsTransactional = false,
                });
                await _csvService.ImportCsvData(tableName, data);
                await newUow.CompleteAsync();
            }
            else
            {
                await _csvService.ReImportCsvData(existing.TableName, data);
                var fileId = _stringEncryptionService.Encrypt(data.FileId.ToString()) ?? "";
                dbId = (await _databaseRepository.FindAsync(x => x.ConnectionString == fileId))?.Id;
            }
            return dbId;
        }

        public async Task<DynamicTable?> GetImportedCsvInfoById(Guid fileId)
        {
            var importer = await _csvImporterRepository.FindAsync(x => x.FileId == fileId);
            return importer == null ? null : new DynamicTable
            {
                FileName = importer.FileName,
                ColumnNames = importer.ColumnNames,
                FileId = fileId,
                FileType = importer.FileType,
                FileSize = importer.FileSize,
                Delimiter = importer.Delimiter
            };
        }
    }

}