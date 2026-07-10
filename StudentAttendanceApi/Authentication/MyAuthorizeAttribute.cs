namespace StudentAttendanceApi.Authentication
{
    public class RoleBasedBasicAuthenticationWEBAPI
    {
        public class MyAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            public MyAuthorizeAttribute(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }
            // 401 (Unauthorized) - indicates that the request has not been applied because it lacks valid 
            // authentication credentials for the target resource.
            // 403 (Forbidden) - when the user is authenticated but isn’t authorized to perform the requested 
            // operation on the given resource.

           
            protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    base.HandleUnauthorizedRequest(actionContext);
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                }
            }

            
        }
    }
}
