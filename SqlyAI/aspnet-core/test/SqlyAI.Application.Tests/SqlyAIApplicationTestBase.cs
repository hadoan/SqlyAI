using Volo.Abp.Modularity;

namespace SqlyAI;

public abstract class SqlyAIApplicationTestBase<TStartupModule> : SqlyAITestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
