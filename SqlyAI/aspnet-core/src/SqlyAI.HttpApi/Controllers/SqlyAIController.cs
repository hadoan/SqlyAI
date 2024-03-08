using SqlyAI.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SqlyAI.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SqlyAIController : AbpControllerBase
{
    protected SqlyAIController()
    {
        LocalizationResource = typeof(SqlyAIResource);
    }
}
