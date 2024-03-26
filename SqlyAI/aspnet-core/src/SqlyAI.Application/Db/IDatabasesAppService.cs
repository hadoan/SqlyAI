namespace SqlyAI.Application.Db
{
    public interface IDatabasesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDatabaseForViewDto>> GetAll(GetAllDatabasesInput input);

        Task<GetDatabaseForViewDto> GetDatabaseForView(Guid id);

        Task<GetDatabaseForEditOutput> GetDatabaseForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDatabaseDto input);

        Task Delete(EntityDto<Guid> input);

        Task<List<QueryDatabaseLookupTableDto>> GetAllDatabaseForTableDropdown();
    }
}