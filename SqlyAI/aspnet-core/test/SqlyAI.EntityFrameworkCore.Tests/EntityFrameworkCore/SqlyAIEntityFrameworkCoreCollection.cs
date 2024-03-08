using Xunit;

namespace SqlyAI.EntityFrameworkCore;

[CollectionDefinition(SqlyAITestConsts.CollectionDefinitionName)]
public class SqlyAIEntityFrameworkCoreCollection : ICollectionFixture<SqlyAIEntityFrameworkCoreFixture>
{

}
