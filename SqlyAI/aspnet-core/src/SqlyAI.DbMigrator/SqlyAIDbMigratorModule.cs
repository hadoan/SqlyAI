using SqlyAI.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace SqlyAI.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SqlyAIEntityFrameworkCoreModule),
    typeof(SqlyAIApplicationContractsModule)
    )]
public class SqlyAIDbMigratorModule : AbpModule
{
}
