using Volo.Abp.Modularity;

namespace SqlyAI;

[DependsOn(
    typeof(SqlyAIDomainModule),
    typeof(SqlyAITestBaseModule)
)]
public class SqlyAIDomainTestModule : AbpModule
{

}
