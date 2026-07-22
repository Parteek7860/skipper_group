using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace parle_new.mainclass
{
    public class LoginFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var path = context.HttpContext.Request.Path.Value?.ToLower();

            // Sirf BackOffice URLs par check
            if (path.StartsWith("/backoffice"))
            {
                
                if (path == "/backoffice/signin" ||
                    path == "/backoffice/dashboard")
                {
                    return;
                }

                // Session check
                var userId = context.HttpContext.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    context.Result = new RedirectToActionResult(
                        "Signin",
                        "backoffice",   
                        null);

                    return;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
