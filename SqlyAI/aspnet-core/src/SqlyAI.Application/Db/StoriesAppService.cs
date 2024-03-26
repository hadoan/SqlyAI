

namespace SqlyAI.Application.Db
{
    public class StoriesAppService : SqlyAIAppService
    {
        private readonly IRepository<Story, Guid> _storyRepository;
        private readonly IRepository<Query, Guid> _queryRepository;
        private readonly IRepository<Database, Guid> _databaseRepository;
        private readonly IRepository<CsvImporter, Guid> _csvImporterRepository;

        public StoriesAppService(IRepository<Story, Guid> storyRepository, IRepository<Database, Guid> databaseRepository, IRepository<Query, Guid> queryRepository, IRepository<CsvImporter, Guid> csvImporterRepository)
        {
            _storyRepository = storyRepository;
            _databaseRepository = databaseRepository;
            _queryRepository = queryRepository;
            _csvImporterRepository = csvImporterRepository;
        }

        public async Task<PagedResultDto<GetStoryForViewDto>> GetAll(GetAllStoriesInput input)
        {
            //var ouIds = await GetCurrentUserOUs();
            var query = await _storyRepository.GetQueryableAsync();
            var queries = query
                        .Include(e => e.Database).Include(x => x.OrganizationUnit)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DatabaseNameFilter), e => e.Database != null && e.Database.Name == input.DatabaseNameFilter)
                        .WhereIf(input.DbType != null && input.DbType != DbType.Unknown, e => e.Database.Type == input.DbType);

            var stories = await queries.OrderBy(input.Sorting ?? "id desc")
                         .PageBy(input)
                         .ToListAsync();

            return new PagedResultDto<GetStoryForViewDto>
            {
                TotalCount = await queries.CountAsync(),
                Items = stories.Select(x => new GetStoryForViewDto
                {
                    Story = new StoryDto { Id = x.Id, DatabaseId = x.DatabaseId, Name = x.Name },
                    DatabaseName = x.Database?.Name,
                    OrganizationUnitName = x.OrganizationUnit?.DisplayName,
                }).ToList()
            };

        }

        public async Task<GetStoryForEditOutput> GetCsvStoryForEdit(EntityDto<Guid> csvId)
        {
            var db = await _databaseRepository.FindAsync(x => (x.Type == DbType.CSV || x.Type == DbType.GoogleSheet) && x.Id == csvId.Id);
            if (db == null)
            {
                throw new UserFriendlyException($"Can't find database with Id {csvId.Id}");
            }
            var story = await (await _storyRepository.GetQueryableAsync())
                .Include(x => x.Queries)
                .FirstAsync(x => x.DatabaseId == csvId.Id);

            if (story == null)
            {
                story = new Story(GuidGenerator.Create())
                {
                    TenantId = CurrentTenant.Id,
                    Name = db.Name,
                    DatabaseId = csvId.Id,
                    QueryTables = new List<string> { db.Name }
                };
                await _storyRepository.InsertAsync(story);
            }

            var output = new GetStoryForEditOutput { Story = ObjectMapper.Map<Story, CreateOrEditStoryDto>(story) };

            if (output.Story.DatabaseId != null)
            {
                var _lookupDatabase = await _databaseRepository.FindAsync(output.Story.DatabaseId ?? Guid.Empty);
                output.DatabaseName = _lookupDatabase?.Name?.ToString();
            }
            if (output.Story.Queries.Count == 0)
            {
                output.Story.Queries.Add(new CreateOrEditQueryDto
                {
                    Name = "Count",
                    Text = "How many records",
                });
            }


            return output;
        }

        public async Task<GetStoryForEditOutput> GetStoryForEdit(EntityDto<Guid> input)
        {
            var query = await _storyRepository.GetQueryableAsync();
            var story = await query
                .Include(x => x.Queries)
                .FirstAsync(x => x.Id == input.Id);
            //await GuardValidOUForCurrentUser(story);
            var output = new GetStoryForEditOutput { Story = ObjectMapper.Map<Story, CreateOrEditStoryDto>(story) };

            if (output.Story.DatabaseId != null)
            {
                var _lookupDatabase = await _databaseRepository.FindAsync(output.Story.DatabaseId ?? Guid.Empty);
                output.DatabaseName = _lookupDatabase?.Name?.ToString();
            }
            //if (output.Story.OrganizationUnitId != null)
            //{
            //    var ou = await OrganizationUnitRepository.FirstOrDefaultAsync((int)output.Story.OrganizationUnitId);
            //    output.OrganizationUnitName = ou?.DisplayName;
            //}

            return output;
        }

        public async Task<Guid> CreateOrEdit(CreateOrEditStoryDto input)
        {
            if (input.Id == null )
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }


        protected virtual async Task<Guid> Create(CreateOrEditStoryDto input)
        {
            var story = ObjectMapper.Map<CreateOrEditStoryDto,Story >(input);

            foreach (var query in story.Queries)
            {
                query.TenantId = CurrentTenant.Id;
                query.Story = story;
                story.TenantId = CurrentTenant.Id;
            }
            var newQueries = story.Queries.Where(x => x.Id == Guid.Empty).ToList();
            var existingQueries = story.Queries.Where(x => x.Id != Guid.Empty);
            story.Queries = newQueries;
            await _storyRepository.InsertAsync(story);
            foreach (var query in existingQueries)
            {
                query.StoryId = story.Id;
                query.TenantId = CurrentTenant.Id;
                await _queryRepository.UpdateAsync(query);
            }
            return story.Id;
        }

        protected virtual async Task<Guid> Update(CreateOrEditStoryDto input)
        {
            var story =await (await _storyRepository.GetQueryableAsync()).FirstAsync(x => x.Id == input.Id);
            story.Name = input.Name;
            story.DatabaseId = input.DatabaseId;
            story.OrganizationUnitId = input.OrganizationUnitId;
            story.QueryTables = input.QueryTables;
            foreach (var query in story.Queries)
            {
                query.TenantId = CurrentTenant.Id;
                query.StoryId = story.Id;
            }
            await _storyRepository.UpdateAsync(story);
            return story.Id;
        }

    }
}