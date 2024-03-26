using CsvHelper;
using System.Linq;

namespace SqlyAI.Application.Db
{
    public class QueriesAppService : SqlyAIAppService, IQueriesAppService
    {
        private readonly IRepository<Query, Guid> _queryRepository;
        private readonly IRepository<Database, Guid> _lookup_databaseRepository;
        private readonly IRepository<QueryMetaData, Guid> _queryMetaDataRepository;

        public QueriesAppService(IRepository<Query, Guid> queryRepository, IRepository<Database, Guid> lookup_databaseRepository, IRepository<QueryMetaData, Guid> queryMetaDataRepository)
        {
            _queryRepository = queryRepository;
            _lookup_databaseRepository = lookup_databaseRepository;
            _queryMetaDataRepository = queryMetaDataRepository;
        }

        public async Task<QueryMetaDataDto> UpdateColumnType(Guid queryId, string columnName, ColumnType type)
        {
            var metaData = await (await _queryMetaDataRepository.GetQueryableAsync()).FirstOrDefaultAsync(x => x.QueryId == queryId);
            if (metaData == null) metaData = new QueryMetaData();
            var config = metaData.ColumnConfigs.Where(x => x.ColumnName == columnName).FirstOrDefault();

            if (config == null)
            {
                config = new ColumnConfig { ColumnName = columnName, ColumnType = type };
                metaData.ColumnConfigs.Add(config);
            }
            else
            {
                config.ColumnType = type;
            }


            await _queryMetaDataRepository.UpdateAsync(metaData);
            var metaDto = ObjectMapper.Map<QueryMetaData, QueryMetaDataDto>(metaData);
            return metaDto;
        }

        public async Task<QueryMetaDataDto> UpdateReportConfiguration(ReportConfigurationInput input)
        {
            var metaData = await (await _queryMetaDataRepository.GetQueryableAsync()).FirstOrDefaultAsync(x => x.QueryId == input.QueryId);
            if (metaData == null) metaData = new QueryMetaData();
            var reportType = input.ReportType;
            var config = metaData.ReportConfigs.Where(x => x.Type.Equals(reportType)).FirstOrDefault();

            if (config == null)
            {
                config = new ReportConfig();
                SetConfigProperties(input, config);
                var configs = metaData.ReportConfigs;
                configs.Add(config);
                metaData.ReportConfigs = configs;
            }
            else
            {
                SetConfigProperties(input, config);
            }

            //update default value for other type 
            var otherReportTypes = ((ReportType[])Enum.GetValues(typeof(ReportType))).Where(x => !x.Equals(input.ReportType));
            foreach (var otherReportType in otherReportTypes)
            {
                var otherReportConfig = metaData.ReportConfigs.Where(x => x.Type.Equals(otherReportType)).FirstOrDefault();
                if (otherReportConfig == null)
                {
                    otherReportConfig = new ReportConfig();
                    SetConfigProperties(input, otherReportConfig);
                    otherReportConfig.Type = otherReportType;
                    var configs = metaData.ReportConfigs;
                    configs.Add(otherReportConfig);
                    metaData.ReportConfigs = configs;
                }
            }

            await _queryMetaDataRepository.UpdateAsync(metaData);
            var metaDto = ObjectMapper.Map<QueryMetaData, QueryMetaDataDto>(metaData);
            return metaDto;


            static void SetConfigProperties(ReportConfigurationInput input, ReportConfig config)
            {
                config.Type = input.ReportType;
                config.XAxisColumn = input.XAxisColumn;
                config.XAxisCalculationMethod = input.XAxisCalculationMethod;
                config.YAxisColumn = input.YAxisColumn;
                config.YAxisCalculationMethod = input.YAxisCalculationMethod;
            }
        }

        public async Task<PagedResultDto<GetQueryForViewDto>> GetAll(GetAllQueriesInput input)
        {
            var query = await _queryRepository.GetQueryableAsync();
            var filteredQueries = query
                        .Where(x => x.StoryId == null)
                        .Include(e => e.Database)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Text.Contains(input.Filter) || e.Sql.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TextFilter), e => e.Text.Contains(input.TextFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SqlFilter), e => e.Sql.Contains(input.SqlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DatabaseNameFilter), e => e.Database != null && e.Database.Name == input.DatabaseNameFilter)
                        .WhereIf(input.DbType != null, e => e.Database.Type == input.DbType);


            var pagedAndFilteredQueries = filteredQueries
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var queries = from o in pagedAndFilteredQueries
                          join o1 in (await _lookup_databaseRepository.GetQueryableAsync()) on o.DatabaseId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          select new
                          {
                              o.DatabaseId,
                              o.Name,
                              o.Text,
                              o.Sql,
                              o.MetaData,
                              Id = o.Id,
                              DatabaseName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                          };

            var totalCount = await filteredQueries.CountAsync();

            var dbList = await queries.ToListAsync();
            var results = new List<GetQueryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetQueryForViewDto()
                {
                    Query = new QueryDto
                    {
                        DatabaseId = o.DatabaseId,
                        Name = o.Name,
                        Text = o.Text,
                        Sql = o.Sql,
                        MetaData = ObjectMapper.Map<QueryMetaData, QueryMetaDataDto>(o.MetaData),
                        Id = o.Id,
                    },
                    DatabaseName = o.DatabaseName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetQueryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetQueryForViewDto> GetQueryForView(Guid id)
        {
            var query = await _queryRepository.GetAsync(id);
            var output = new GetQueryForViewDto { Query = ObjectMapper.Map<Query, QueryDto>(query) };

            if (output.Query.DatabaseId != null)
            {
                var _lookupDatabase = await _lookup_databaseRepository.FindAsync(output.Query.DatabaseId ?? Guid.Empty);
                output.DatabaseName = _lookupDatabase?.Name?.ToString();
            }


            return output;
        }

        public async Task<GetQueryForEditOutput> GetQueryForEdit(EntityDto<Guid> input)
        {
            var query = await _queryRepository.FindAsync(input.Id);
            var output = new GetQueryForEditOutput { Query = ObjectMapper.Map<Query, CreateOrEditQueryDto>(query) };

            if (output.Query.DatabaseId != null)
            {
                var _lookupDatabase = await _lookup_databaseRepository.FindAsync(x=>x.Id == output.Query.DatabaseId);
                output.DatabaseName = _lookupDatabase?.Name?.ToString();
            }
            //if (output.Query.OrganizationUnitId != null)
            //{
            //    var ou = await OrganizationUnitRepository.FirstOrDefaultAsync(output.Query.OrganizationUnitId);
            //    output.OrganizationUnitName = ou?.DisplayName;
            //}
            var metaData = await (await _queryMetaDataRepository.GetQueryableAsync()).FirstOrDefaultAsync(x => x.QueryId == input.Id);
            if (metaData != null)
            {
                var metaDto = ObjectMapper.Map<QueryMetaData, QueryMetaDataDto>(metaData);
                output.MetaData = metaDto;
                return output;
            }
            return null;
        }

        public async Task<QueryMetaDataDto> GetQueryMetaData(EntityDto<Guid> input)
        {
            var metaData = await (await _queryMetaDataRepository.GetQueryableAsync()).FirstOrDefaultAsync(x => x.QueryId == input.Id);
            var metaDto = ObjectMapper.Map<QueryMetaData, QueryMetaDataDto>(metaData);
            foreach (var reportConfig in metaData.ReportConfigs)
            {
                var config = new ReportConfigDto()
                {
                    Type = reportConfig.Type,
                    XAxisCalculationMethod = reportConfig.XAxisCalculationMethod.ToString(),
                    XAxisColumn = reportConfig.XAxisColumn,
                    YAxisCalculationMethod = reportConfig.YAxisCalculationMethod.ToString(),
                    YAxisColumn = reportConfig.YAxisColumn,
                };
                metaDto.ReportConfigurations.Add(config);
            }
            return metaDto;
        }


        public async Task<Guid> CreateOrEdit(CreateOrEditQueryDto input)
        {
            if (!string.IsNullOrEmpty(input.Sql))
            {
                input.Sql = input.Sql.Trim().Trim(Environment.NewLine.ToArray());
            }
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        protected virtual async Task<Guid> Create(CreateOrEditQueryDto input)
        {
            var query = ObjectMapper.Map<CreateOrEditQueryDto, Query>(input);

            if (CurrentTenant.Id != null)
            {
                query.TenantId = CurrentTenant.Id;
            }

            await _queryRepository.InsertAsync(query);
            return query.Id;
        }

        protected virtual async Task<Guid> Update(CreateOrEditQueryDto input)
        {
            var query = await _queryRepository.FindAsync(x => x.Id == input.Id);

            ObjectMapper.Map(input, query);
            await _queryRepository.UpdateAsync(query);
            return query.Id;
        }

        public async Task Delete(EntityDto<Guid> input)
        {
            var query = await _queryRepository.FindAsync(input.Id);
            await _queryRepository.DeleteAsync(input.Id);
        }


    }
}