using SqlyAI.Samples;
using Xunit;

namespace SqlyAI.EntityFrameworkCore.Applications;

[Collection(SqlyAITestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SqlyAIEntityFrameworkCoreTestModule>
{

}
