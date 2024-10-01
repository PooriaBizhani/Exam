using First_Sample.Domain.InterFaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Security.Cryptography;

namespace First_Sample.Application.Security.DynamicRole
{
    public class DynamicRoleHandler :  AuthorizationHandler<DynamicRoleRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUtilities _Utilities;
        private readonly IMemoryCache _memoryCache;
        private readonly IDataProtector _dataProtector;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public DynamicRoleHandler(IHttpContextAccessor httpContextAccessor, IUtilities utilities, IMemoryCache memoryCache, IDataProtectionProvider dataProtectionProvider,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _contextAccessor = httpContextAccessor;
            _Utilities = utilities;
            _memoryCache = memoryCache;
            _dataProtector = dataProtectionProvider.CreateProtector("RvgGuid");
            _signInManager = signInManager;
            _userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRoleRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return;

            var dbRoleValidationGuid = _memoryCache.GetOrCreate("dbRoleValidationGuid", p =>
            {
                p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                return _Utilities.DataBaseRoleValidationGuid();
            });

            var allAreasName = _memoryCache.GetOrCreate("AllAreasName", p =>
            {
                p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                return _Utilities.GetAllAreasNames();
            });

            SplitUserRequestedUrl(httpContext.Request.Path.ToString(), allAreasName, out var areaAndActionAndControllerName);

            UnprotectRvgCookieData(httpContext, out var unProtectedRvgCookie);

            if (!IsRvgCookieDataValid(unProtectedRvgCookie, userId, dbRoleValidationGuid))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return;
                AddOrUpdateRvgCookie(httpContext, userId, dbRoleValidationGuid);

                await _signInManager.RefreshSignInAsync(user);
            }
            else if(httpContext.User.HasClaim(areaAndActionAndControllerName,true.ToString()))
            {
                context.Succeed(requirement);
            }
            return;
        }
        private void SplitUserRequestedUrl(string url, IList<string> areaNames,
         out string areaAndControllerAndActionName)
        {
            var requestedUrl = url.Split('/')
                .Where(t => !string.IsNullOrEmpty(t)).ToList();
            var urlCount = requestedUrl.Count;
            if (urlCount != 0 &&
                areaNames.Any(t => t.Equals(requestedUrl[0], StringComparison.CurrentCultureIgnoreCase)))
            {
                var areaName = requestedUrl[0];
                var controllerName = (urlCount == 1) ? "HomeController" : requestedUrl[1] + "Controller";
                var actionName = (urlCount > 2) ? requestedUrl[2] : "Index";
                areaAndControllerAndActionName = $"{areaName}|{controllerName}|{actionName}".ToUpper();
            }
            else
            {
                var areaName = "NoArea";
                var controllerName = (urlCount == 0) ? "HomeController" : requestedUrl[0] + "Controller";
                var actionName = (urlCount > 1) ? requestedUrl[1] : "Index";
                areaAndControllerAndActionName = $"{areaName}|{controllerName}|{actionName}".ToUpper();
            }
        }
        private void UnprotectRvgCookieData(HttpContext HttpContext, out string unProtectedRvgCookie)
        {
            var protectedRvCookie = HttpContext.Request.Cookies.FirstOrDefault(o => o.Key == "RVG").Value;
            unProtectedRvgCookie = null;
            if (!string.IsNullOrEmpty(protectedRvCookie))
            {
                try
                {
                    unProtectedRvgCookie = _dataProtector.Unprotect(protectedRvCookie);
                }
                catch (CryptographicException)
                {
                }
            }
        }

        private bool IsRvgCookieDataValid(string RvgCookieData, string validUserId, string validRvg)
            => !string.IsNullOrEmpty(RvgCookieData) &&
            SplitUserIdFromRvgCookie(RvgCookieData) == validUserId &&
            SplitRvgFromRvgCookie(RvgCookieData) == validRvg;

        private string SplitUserIdFromRvgCookie(string RvgCookieData) => RvgCookieData.Split("|||")[0];

        private string SplitRvgFromRvgCookie(string RvgCookieData) => RvgCookieData.Split("|||")[1];
        private string CombineRvgWithUserId(string rvg, string userId) => rvg + "|||" + userId;

        private void AddOrUpdateRvgCookie(HttpContext httpContext , string validRvg , string validUserId)
        {
            var rvgWithUserId = CombineRvgWithUserId(validRvg, validUserId);

            var protectedRvgWihUserId = _dataProtector.Protect(rvgWithUserId);

            httpContext.Response.Cookies.Append("RVG",protectedRvgWihUserId, new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(90),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });
        }
    }
}
