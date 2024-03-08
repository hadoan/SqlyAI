using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SqlyAI;

[Dependency(ReplaceServices = true)]
public class SqlyAIBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "SqlyAI";
}
