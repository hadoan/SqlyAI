using System.Threading.Tasks;

namespace SqlyAI.Data;

public interface ISqlyAIDbSchemaMigrator
{
    Task MigrateAsync();
}
