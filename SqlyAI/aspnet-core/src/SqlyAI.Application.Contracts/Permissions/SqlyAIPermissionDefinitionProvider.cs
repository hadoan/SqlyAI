using SqlyAI.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SqlyAI.Permissions;

public class SqlyAIPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SqlyAIPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(SqlyAIPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SqlyAIResource>(name);
    }
}
