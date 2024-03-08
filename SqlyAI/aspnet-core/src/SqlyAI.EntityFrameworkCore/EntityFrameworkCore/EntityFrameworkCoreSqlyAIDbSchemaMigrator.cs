using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqlyAI.Data;
using Volo.Abp.DependencyInjection;

namespace SqlyAI.EntityFrameworkCore;

public class EntityFrameworkCoreSqlyAIDbSchemaMigrator
    : ISqlyAIDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSqlyAIDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the SqlyAIDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SqlyAIDbContext>()
            .Database
            .MigrateAsync();
    }
}
