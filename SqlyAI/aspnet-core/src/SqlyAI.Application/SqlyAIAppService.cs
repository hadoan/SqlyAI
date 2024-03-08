using System;
using System.Collections.Generic;
using System.Text;
using SqlyAI.Localization;
using Volo.Abp.Application.Services;

namespace SqlyAI;

/* Inherit your application services from this class.
 */
public abstract class SqlyAIAppService : ApplicationService
{
    protected SqlyAIAppService()
    {
        LocalizationResource = typeof(SqlyAIResource);
    }
}
