using static OnguardImportManager.Permissions.PermissionConstants;

namespace OnguardImportManager.Permissions
{
    public interface IPermissionService
    {
        string GetDbConnectionString();

        string GetRole();
        int GetClientId();
        string GetClientSsisFolder();
        int GetUserId();
        string GetImportManagerVersion();

        bool HasPermission(ApplicationModules applicationModule,
                           ApplicationPermissions permissionName);

    }
}
