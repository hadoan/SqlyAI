using Volo.Abp.Modularity;

namespace SqlyAI;

/* Inherit from this class for your domain layer tests. */
public abstract class SqlyAIDomainTestBase<TStartupModule> : SqlyAITestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
