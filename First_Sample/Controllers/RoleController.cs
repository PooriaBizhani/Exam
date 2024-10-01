using First_Sample.Domain.InterFaces;
using First_Sample.Domain.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Security.Claims;

namespace First_Sample.Presentation.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUtilities _utilities;
        private readonly IMemoryCache _memoryCache;

        public RoleController(RoleManager<IdentityRole> roleManager,
            IUtilities utilities, IMemoryCache memoryCache)
        {
            _utilities = utilities;
            _roleManager = roleManager;
            _memoryCache = memoryCache;
        }
        public async Task<IActionResult> Index()
        {
            var roles = _roleManager.Roles.ToList();
            var model = new List<IndexVM>();
            foreach (var role in roles)
            {
                model.Add(new IndexVM()
                {
                    RoleName = role.Name,
                    RoleId = role.Id,
                });
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            var allProjectNames = _memoryCache.GetOrCreate("AreaAndActionAndControllerNamesList", entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.MaxValue;
                var controllerActionList = new List<ActionAndControllerName>();

                var controllers = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type) && !type.IsAbstract)
                    .ToList();

                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                        .Where(m => !m.IsDefined(typeof(NonActionAttribute)));

                    foreach (var action in actions)
                    {
                        controllerActionList.Add(new ActionAndControllerName
                        {
                            ControllerName = controller.Name,
                            ActionName = action.Name,
                            AreaName = "NoArea" // اگر نیاز دارید، می‌توانید اینجا منطقی برای تعیین AreaName اضافه کنید
                        });
                    }
                }

                return controllerActionList; // بازگشت لیست کنترلرها و اکشن‌ها
            });

            var model = new CreateRoleVM
            {
                ActionAndControllerNames = allProjectNames // افزودن لیست به مدل
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(model.RoleName);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    var requestRols = model.ActionAndControllerNames.Where(o => o.IsSelected).ToList();
                    foreach (var item in requestRols)
                    {
                        var areaName = (string.IsNullOrEmpty(item.AreaName))?
                            "NoArea" : item.AreaName;
                        await _roleManager.AddClaimAsync(role, new Claim($"{areaName}|{item.ControllerName}|{item.ActionName}".ToUpper(),true.ToString()));
                    }

                    RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }
    }
}
