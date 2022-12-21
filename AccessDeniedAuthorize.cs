using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Providers
{
    public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.Result is UnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/AccessDenied");
            }
        }
    }
}