using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System;

namespace StudentAttendanceApi.ActivityLog
{
    public class UserActivityFilter : IActionFilter
    {
        private IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private ILogUserActivityService _service;

        //injecting specified service to insert the log in database.


        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    // Stores the Request in an Accessible object
        //    var request = filterContext.HttpContext.Request;
        //    // Generate the log of user activity

        //    LogUserActivity log = new LogUserActivity()
        //    {
        //        // Username. shall be changed to UserId later afteter Auth management.
        //        UserName = filterContext.HttpContext.User.Identity.Name ?? "Anonymous",
        //        // The IP Address of the Request
        //        IpAddress = request.HttpContext.Connection.RemoteIpAddress.ToString(),
        //        // The URL that was accessed
        //        AreaAccessed = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request),
        //        // Creates Timestamp
        //        TimeStamp = DateTime.Now
        //    };

        //    //saves the log to database
        //    _service.SaveLog(log);

        //    // Finishes executing the Action as normal 
        //    base.OnActionExecuting(filterContext);
        //}

        private readonly AppDbContext appDbContext;

        public UserActivityFilter(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.appDbContext = appDbContext;
            _httpContextAccessor=httpContextAccessor;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var data = "";
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            var url = $"{controllerName}/{actionName}";

            if (!string.IsNullOrEmpty(context.HttpContext.Request.QueryString.Value))
            {
                data = context.HttpContext.Request.QueryString.Value;
            }
            else
            {
                var userData = context.ActionArguments.FirstOrDefault();
                var stringUserData = JsonConvert.SerializeObject(userData);
                data = stringUserData;
            }

            //var userId = context.HttpContext.User.Identity.
            var userId=_httpContextAccessor.HttpContext.User.Claims
                                .FirstOrDefault(c => c.Type == "UserId")?.Value;
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();

            StoreUserActivity(data, url, userName, ipAddress);

        }

        public void StoreUserActivity(string data, string url, string userName, string ipAddress)
        {
            try
            {
                var userActivity = new UserActivityLog
                {
                    UserName = userName,
                    IpAddress = ipAddress,
                    Data = data,
                    Url = url
                };

                appDbContext.UserActivityLog.Add(userActivity);
                appDbContext.SaveChanges();
            }catch(Exception ex)
            {

            }
        }

    }
}
