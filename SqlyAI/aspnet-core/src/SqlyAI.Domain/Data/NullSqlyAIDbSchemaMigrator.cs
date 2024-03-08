using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SqlyAI.Data;

/* This is used if database provider does't define
 * ISqlyAIDbSchemaMigrator implementation.
 */
public class NullSqlyAIDbSchemaMigrator : ISqlyAIDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
