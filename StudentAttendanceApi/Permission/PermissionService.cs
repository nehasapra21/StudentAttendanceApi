using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using static OnguardImportManager.Permissions.PermissionConstants;

namespace OnguardImportManager.Permissions
{
    public class PermissionService : IPermissionService
    {
        private string ConnectionString;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private IConfiguration _configuration { get; }

        public PermissionService(IHttpContextAccessor httpContextAccessor,
                                 IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string GetDbConnectionString()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString = _configuration.GetConnectionString("IMDatabase");
            }

            return ConnectionString;
        }

        public string GetRole()
        {
            var role = _httpContextAccessor.HttpContext.User.Claims
                                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return role;
        }

        public int GetClientId()
        {
            var id = _httpContextAccessor.HttpContext.User.Claims
                                .FirstOrDefault(c => c.Type == "ClientId")?.Value;
            if (!string.IsNullOrEmpty(id))
            {
                return Convert.ToInt32(id);
            }
            else
            {
                return 0;
            }
        }

        public string GetClientSsisFolder()
        {
            var ClientSsisCatalogFolder = _httpContextAccessor.HttpContext.User.Claims
                                .FirstOrDefault(c => c.Type == "ClientSsisCatalogFolder")?.Value;
            return ClientSsisCatalogFolder;
        }

        public int GetUserId()
        {
            var id = _httpContextAccessor.HttpContext.User.Claims
                                .FirstOrDefault(c => c.Type == "UserId")?.Value;
            int userId = Convert.ToInt32(id);
            return userId;
        }

        public string GetImportManagerVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string importManagerVersion = string.Empty;

            if (assembly != null)
            {
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    importManagerVersion = ((AssemblyFileVersionAttribute)customAttributes[0]).Version;
                }
            }
            return importManagerVersion;
        }

        public bool HasPermission(ApplicationModules applicationModule,
                                  ApplicationPermissions permissionName)
        {
            var role = _httpContextAccessor.HttpContext.User.Claims
                                 .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == RoleAdminName)
            {
                return true;
            }
            else if (role == RoleConsultantName)
            {
                return HasPermissionForConsultantOrSupport(applicationModule, permissionName);
            }
            else if (role == RoleSupportName)
            {
                return HasPermissionForConsultantOrSupport(applicationModule, permissionName);
            }
            else if (role == RoleViewerName)
            {
                return HasPermissionViewer(applicationModule, permissionName);
            }
            else if (role == RoleDevOpsName)
            {
                return HasPermissionForDevops(applicationModule, permissionName);
            }
            else
            {
                return false;
            }
        }



        private bool HasPermissionForConsultantOrSupport(ApplicationModules applicationModule,
                                           ApplicationPermissions permissionName)
        {
            bool hasPermission = true;

            if (applicationModule == ApplicationModules.Users && permissionName == ApplicationPermissions.Add
                ||
                ((applicationModule == ApplicationModules.Template || applicationModule == ApplicationModules.Connections) 
                && GetClientId() == 1))
            {
                hasPermission = false;
            }
           
            return hasPermission;
        }

        private bool HasPermissionViewer(ApplicationModules applicationModule,
                                          ApplicationPermissions permissionName)
        {
            bool hasPermission = false;

            if (applicationModule == ApplicationModules.Template &&
               permissionName == ApplicationPermissions.ExecuteTemplate)
            {
                return true;
            }
            return hasPermission;
        }

        private bool HasPermissionForDevops(ApplicationModules applicationModule,
                                         ApplicationPermissions permissionName)
        {
            bool hasPermission = false;

            if (applicationModule == ApplicationModules.Template
                      && (permissionName == ApplicationPermissions.TemplateSetting
                      || permissionName == ApplicationPermissions.ScheduleTemplate
                      || permissionName == ApplicationPermissions.ExecuteTemplate))
            {
                hasPermission = true;
            }
            else if (applicationModule == ApplicationModules.Users || applicationModule == ApplicationModules.EmailServerSettings ||
                    applicationModule == ApplicationModules.ScheduledPackages || applicationModule == ApplicationModules.ClientConnections)
            {
                hasPermission = true;
            }
            return hasPermission;
        }
    }
}
