using Volo.Abp.Modularity;

namespace SqlyAI;

[DependsOn(
    typeof(SqlyAIApplicationModule),
    typeof(SqlyAIDomainTestModule)
)]
public class SqlyAIApplicationTestModule : AbpModule
{

}
