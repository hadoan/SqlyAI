using SqlyAI.Samples;
using Xunit;

namespace SqlyAI.EntityFrameworkCore.Domains;

[Collection(SqlyAITestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SqlyAIEntityFrameworkCoreTestModule>
{

}
