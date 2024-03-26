
namespace SqlyAI.Application.Db
{
    public interface IQueriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetQueryForViewDto>> GetAll(GetAllQueriesInput input);

        Task<GetQueryForViewDto> GetQueryForView(Guid id);

        Task<GetQueryForEditOutput> GetQueryForEdit(EntityDto<Guid> input);

        Task<Guid> CreateOrEdit(CreateOrEditQueryDto input);

        Task Delete(EntityDto<Guid> input);


    }
}