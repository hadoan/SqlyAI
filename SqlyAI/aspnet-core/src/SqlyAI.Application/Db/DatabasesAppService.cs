
//namespace SqlyAI.Application.Db
//{
//    public class DatabasesAppService : SqlyAIAppService, IDatabasesAppService
//    {
//        private readonly IRepository<Database, Guid> _databaseRepository;
//        private readonly IDatabaseInfoService _databaseInfoService;
//        private readonly IRepository<CsvImporter, Guid> _csvImporterRepository;
//        private readonly DatabaseManager _databaseManager;

//        private readonly string csvDbConnectionString;

//        public DatabasesAppService(IRepository<Database, Guid> databaseRepository, IDatabaseInfoService databaseInfoService, IAppIntegrationConfigurationAccessor configuration, IRepository<CsvImporter, Guid> csvImporterRepository, DatabaseManager databaseManager)
//        {
//            _databaseRepository = databaseRepository;
//            _databaseInfoService = databaseInfoService;
//            csvDbConnectionString = configuration.Configuration["ConnectionStrings:CsvDb"];
//            _csvImporterRepository = csvImporterRepository;
//            _databaseManager = databaseManager;
//        }

//        public async Task<PagedResultDto<GetDatabaseForViewDto>> GetAll(GetAllDatabasesInput input)
//        {
//            var typeFilter = input.TypeFilter.HasValue
//                        ? (DbType)input.TypeFilter
//                        : default;
//            var query = await _databaseRepository.GetQueryableAsync();
//            var filteredDatabases = query
//                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.ConnectionString.Contains(input.Filter))
//                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
//                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
//                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConnectionStringFilter), e => e.ConnectionString.Contains(input.ConnectionStringFilter));

//            var pagedAndFilteredDatabases = filteredDatabases
//                .OrderBy(input.Sorting ?? "id asc")
//                .PageBy(input);

//            var databases = from o in pagedAndFilteredDatabases
//                            select new
//                            {
//                                o.Type,
//                                o.Name,
//                                Id = o.Id,
//                                o.ConnectionString
//                            };

//            var totalCount = await filteredDatabases.CountAsync();

//            var dbList = await databases.ToListAsync();
//            var results = new List<GetDatabaseForViewDto>();

//            foreach (var o in dbList)
//            {
//                var res = new GetDatabaseForViewDto()
//                {
//                    Database = new DatabaseDto
//                    {

//                        Type = o.Type,
//                        Name = o.Name,
//                        ConnectionString = "***",
//                        Id = o.Id,
//                    }
//                };

//                results.Add(res);
//            }
//            foreach (var item in results.Where(x => x.Database.Type == DbType.CSV || x.Database.Type == DbType.GoogleSheet))
//            {
//                item.CsvFileId = dbList.FirstOrDefault(x => x.Id == item.Database.Id)?.ConnectionString;
//            }

//            return new PagedResultDto<GetDatabaseForViewDto>(
//                totalCount,
//                results
//            );

//        }

//        public async Task<GetDatabaseForViewDto> GetDatabaseForView(Guid id)
//        {
//            var database = await _databaseRepository.GetAsync(id);
//            //await GuardValidOUForCurrentUser(database);
//            var output = new GetDatabaseForViewDto { Database = ObjectMapper.Map<Database, DatabaseDto>(database) };
//            output.Database.ConnectionString = "";
//            //if (output.Database.OrganizationUnitId != null)
//            //{
//            //    var ou = await OrganizationUnitRepository.FirstOrDefaultAsync((int)output.Database.OrganizationUnitId);
//            //    output.OrganizationUnitName = ou?.DisplayName;
//            //}
//            return output;
//        }

//        public async Task<GetDatabaseForEditOutput> GetDatabaseForEdit(EntityDto<Guid> input)
//        {
//            var database = await _databaseRepository.GetAsync(input.Id);
//            var output = new GetDatabaseForEditOutput { Database = ObjectMapper.Map<Database, CreateOrEditDatabaseDto>(database) };
//            output.Database.ConnectionString = "";
//            //if (output.Database.OrganizationUnitId != null)
//            //{
//            //    var ou = await OrganizationUnitRepository.FirstOrDefaultAsync((int)output.Database.OrganizationUnitId);
//            //    output.OrganizationUnitName = ou?.DisplayName;
//            //}
//            return output;
//        }

//        public async Task CreateOrEdit(CreateOrEditDatabaseDto input)
//        {
//            if (input.Id == null)
//            {
//                await Create(input);
//            }
//            else
//            {
//                await Update(input);
//            }
//        }

        
//        protected virtual async Task Create(CreateOrEditDatabaseDto input)
//        {
//            var database = ObjectMapper.Map<CreateOrEditDatabaseDto, Database>(input);
            
//            if (CurrentTenant.Id != null)
//            {
//                database.TenantId = CurrentTenant.Id;
//            }
//            await _databaseRepository.InsertAsync(database);

//        }

        
//        protected virtual async Task Update(CreateOrEditDatabaseDto input)
//        {
//            var database = await _databaseRepository.GetAsync(x=>x.Id == input.Id);
//            //await GuardValidOUForCurrentUser(database);
//            ObjectMapper.Map(input, database);

//        }

        
//        public async Task Delete(EntityDto<Guid> input)
//        {
//            var database = await _databaseRepository.GetAsync(input.Id);
//            //await GuardValidOUForCurrentUser(database);
//            await _databaseRepository.DeleteAsync(input.Id);
//        }

//        public async Task<GetDbSchemaInfoOutput> GetDbSchemaInfo(Guid id)
//        {
//            var db = await _databaseRepository.GetAsync(id);
//            //await GuardValidOUForCurrentUser(db);
//            var infoes = await _databaseManager.GetDbSchemaInfoAsync(db);
//            return new GetDbSchemaInfoOutput(infoes.Select(x => x.TableName).Distinct().Order(), infoes);
//        }

        
//        public async Task<List<QueryDatabaseLookupTableDto>> GetAllDatabaseForTableDropdown()
//        {
//            //var ouIds = await GetCurrentUserOUs();
//            var query = await _databaseRepository.GetQueryableAsync();
//            return await query
//                .Select(database => new QueryDatabaseLookupTableDto
//                {
//                    Id = database.Id,
//                    DisplayName = database == null || database.Name == null ? "" : database.Name.ToString(),
//                    Type = database == null ? DbType.Unknown : database.Type
//                }).ToListAsync();
//        }

//    }

//    public record GetDbSchemaInfoOutput(IEnumerable<string> TableNames, IEnumerable<DbSchemaInfo> DbSchema);
//}