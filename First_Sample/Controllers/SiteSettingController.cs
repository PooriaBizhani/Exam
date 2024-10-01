using First_Sample.Application.InterFaces;
using First_Sample.Domain.ViewModels.SiteSetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace First_Sample.Presentation.Controllers
{
    [Authorize(Policy = "DynamicRole")]
    public class SiteSettingController : Controller
    {
        private readonly ISiteSettingService _siteSettingService;
        private readonly IMemoryCache _memoryCache;
        public SiteSettingController(IMemoryCache memoryCache
            ,ISiteSettingService siteSettingService)
        {
            _memoryCache = memoryCache;
            _siteSettingService = siteSettingService;
        }
        public async Task<IActionResult> Index()
        {
            var settings = await _siteSettingService.GetAllService();
            return View(settings);
        }
        [HttpGet]
        public async Task<IActionResult> RoleValidationGuid()
        {
            var roleValidationGuidSiteSetting =
                await _siteSettingService.GetKeyService("RoleValidationGuid");

            var model = new RoleValidationGuidVM()
            {
                LastTimeChenged = roleValidationGuidSiteSetting.LastTimeChenged,
                Value = roleValidationGuidSiteSetting?.Value
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleValidationGuid(RoleValidationGuidVM model)
        {
            var roleValidationGuidSiteSetting =
                await _siteSettingService.GetKeyService("RoleValidationGuid");
            if (roleValidationGuidSiteSetting == null)
            {
                var NewSiteSetting = new SiteSettingVM
                {
                    Key = "RoleValidationGuid",
                    Value = Guid.NewGuid().ToString(),
                    LastTimeChenged = DateTime.Now
                };
              var IsAdded =  await _siteSettingService.AddService(NewSiteSetting);
            }
            else
            {
                var NewSiteSetting = new SiteSettingVM
                {
                    Key = "RoleValidationGuid",
                    Value = roleValidationGuidSiteSetting.Value = Guid.NewGuid().ToString(),
                    LastTimeChenged = roleValidationGuidSiteSetting.LastTimeChenged = DateTime.Now
                };
             var IsUpdate = await _siteSettingService.UpdateService(NewSiteSetting);
            }_memoryCache.Remove("RoleValidationGuid");
                return RedirectToAction("Index");
        }
    }
}
