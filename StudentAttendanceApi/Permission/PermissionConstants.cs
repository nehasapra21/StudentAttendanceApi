namespace OnguardImportManager.Permissions
{
    public static class PermissionConstants
    {
        public const string RoleAdminName = "Admin";
        public const string RoleDevOpsName = "DevOps";
        public const string RoleViewerName = "Viewer";
        public const string RoleConsultantName = "Consultant";
        public const string RoleSupportName = "Support";

        public const string AllDevops = "AllDevops";
        public const string AdminAndDevops = "AdminAndDevops";
        public const string AdminSupportConsultant = "AdminSupportConsultant";
        public const string AdminSupportConsultantDevops = "AdminSupportConsultantDevops";
        public const string All = "All";


        public enum ApplicationModules
        {
            Template = 1,
            Connections = 2,
            DeploymentSettings,
            EmailServerSettings,
            ScheduledPackages,
            Users,
            Roles,
            ClientConnections,
            Client,
            TemplateGroup
        }

        public enum ApplicationPermissions
        {
            Add = 1,
            Edit = 2,
            Delete = 3,
            ImportExportTemplate = 4,
            DeployTemplate = 5,
            ExecuteTemplate = 6,
            CheckValidaionLogTemplate = 7,
            ScheduleTemplate = 8,
            TemplateSetting = 9
        }
    }
}