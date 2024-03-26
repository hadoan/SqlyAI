using System.Reflection.PortableExecutable;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Pillio.BackgrounJobs
{
    public class UpdateQueryLastQueryColumnsJobArgs
    {
        public Guid QueryId { get; set; }
        public List<string> Headers { get; set; }
    }

    public class UpdateQueryLastQueryColumnsJob : AsyncBackgroundJob<UpdateQueryLastQueryColumnsJobArgs>, ITransientDependency
    {
        private IRepository<QueryMetaData, Guid> _metaDataRepository;
        private IUnitOfWorkManager _unitOfWorkManager;

        public UpdateQueryLastQueryColumnsJob(IRepository<QueryMetaData, Guid> metaDataRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _metaDataRepository = metaDataRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override async Task ExecuteAsync(UpdateQueryLastQueryColumnsJobArgs args)
        {
            using var uow = _unitOfWorkManager.Begin();
            var meta = await _metaDataRepository.FindAsync(x => x.QueryId == args.QueryId);
            if (meta == null)
            {
                meta = new QueryMetaData { LastQueryColumns = args.Headers, QueryId = args.QueryId };
                meta.ColumnConfigs = args.Headers.Select(x => new ColumnConfig { ColumnName = x, ColumnType = ColumnType.OTHER}).ToList();
                await _metaDataRepository.InsertAsync(meta);
            }
            else
            {
                meta.LastQueryColumns = args.Headers;
                var newColumns = args.Headers.Where(x => !meta.ColumnConfigs.Any(c => c.ColumnName == x));
                meta.ColumnConfigs.AddRange(newColumns.Select(x => new ColumnConfig { ColumnName = x, ColumnType = ColumnType.OTHER }));
                await _metaDataRepository.UpdateAsync(meta);
            }
            await uow.CompleteAsync();
        }
    }
}
