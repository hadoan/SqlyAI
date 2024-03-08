using Volo.Abp.Settings;

namespace SqlyAI.Settings;

public class SqlyAISettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SqlyAISettings.MySetting1));
    }
}
