using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;


namespace First_Sample.Application.Logs
{

    public class LogActionFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var actionName = context.ActionDescriptor.RouteValues["action"];
            var controllerName = context.ActionDescriptor.RouteValues["controller"];

            // فقط اکشن‌های Login و Logout در کنترلر Account را لاگ‌گذاری کنید
            if (controllerName == "Account" && (actionName == "Login" || actionName == "LogOut"))
            {
                var userId = context.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var userName = context.HttpContext.User.Identity?.Name;


                var ipAddress = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.HttpContext.Connection.RemoteIpAddress?.ToString();


                // ثبت لاگ در Serilog
                Log.ForContext("Action", actionName)
                   .ForContext("Controller", controllerName)
                   .ForContext("UserId", userId)
                   .ForContext("UserName", userName)
                   .Information($"Action Executed: {controllerName}/{actionName} by User {userName} with ID {userId}");
            }

            // برای جلوگیری از خطاهای Async
            await Task.CompletedTask;
        }
    }
}
