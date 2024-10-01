using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace First_Sample.Application.Security.DynamicRole
{
    public class DynamicRoleAuthorizeFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // مسیرهای آزاد برای دسترسی
            var freePaths = new[] { "/", "/account/login", "/account/register" };

            // بررسی وضعیت احراز هویت کاربر
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // اگر مسیر جاری یکی از مسیرهای آزاد باشد، ادامه بده
                var currentPath = context.HttpContext.Request.Path.Value.ToLower();
                if (freePaths.Any(path => currentPath.Contains(path)))
                {
                    return; // اجازه دسترسی به صفحات آزاد
                }

                // هدایت به صفحه ورود
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var authorizationService = context.HttpContext.RequestServices.GetService<IAuthorizationService>();
            var result = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, "DynamicRole");

            if (!result.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
