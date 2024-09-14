using AutoMapper;
using First_Sample.Application.Dtos.User;
using First_Sample.Application.Services;
using First_Sample.Shared.ViewModels.RegisterUser;
using First_Sample.Shared.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace First_Sample.Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly CityService _cityService;
        private readonly ProvinceService _provinceService;
        private readonly IMapper _mapper;

        public UserController(UserService userService, CityService cityService, ProvinceService provinceService)
        {
            _userService = userService;
            _cityService = cityService;
            _provinceService = provinceService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllService(); // دریافت کاربران از سرویس
            var userVMs = users.Select(user => new UserVM
            { 
                Photo = user.Photo,
                Name = user.Name,
                Family = user.Family,
                NationalCode = user.NationalCode,
                CityName = user.CityName,
                ProvinceName = user.ProvinceName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                // سایر فیلدها
            }).ToList(); // تبدیل لیست از User به UserVM
            return View(userVMs); // ارسال به View
        }
        public async Task<IActionResult> Register()
        {
            var provinces = await _provinceService.GetAllService();
            ViewBag.Provinces = provinces.Select(p => new SelectListItem
            {
                Value = p.ProvinceId.ToString(),
                Text = p.Name
            }).ToList();
            var userVM = new RegisterUserVM
            {
                Name = "John",
                Family = "Doe",
                NationalCode = "1234567890",
                PhoneNumber = "09123456789",
                Address = "Sample Address"
            };
            ViewBag.Cities = new List<SelectListItem>();
            return View(userVM);// ارسال به View
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserVM userVM)
        {
            if (!ModelState.IsValid)
            {
                // در صورت بروز خطا دوباره استان‌ها و شهرها را بارگذاری کنید
                var provinces = await _provinceService.GetAllService();
                ViewBag.Provinces = provinces
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProvinceId.ToString(),
                        Text = p.Name
                    })
                    .ToList();
                ViewBag.Cities = new List<SelectListItem>();

                return View(userVM); // بازگشت به فرم ثبت‌نام با نمایش خطاها
            }

            var province = await _provinceService.GetByIdService(userVM.ProvinceId);
            var city = await _cityService.GetByIdService(userVM.CityId);

            string photoPath = null;
            if (userVM.Photo != null && userVM.Photo.Length > 0)
            {
                // تعیین مسیر پوشه و نام یکتا برای فایل
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userVM.Photo.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                // ذخیره فایل در پوشه مشخص شده
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await userVM.Photo.CopyToAsync(stream);
                }

                // ذخیره مسیر فایل به صورت نسبی
                photoPath = Path.Combine("uploads", fileName);
            }

            // ایجاد یک شیء جدید User
            var user = new UserDto
            {
                Name = userVM.Name,
                Family = userVM.Family,
                NationalCode = userVM.NationalCode,
                PhoneNumber = userVM.PhoneNumber,
                ProvinceName = province.Name,
                CityName = city.Name,
                Address = userVM.Address,
                Photo = photoPath // ذخیره مسیر فایل در دیتابیس
            };
            // ذخیره کاربر جدید در دیتابیس از طریق سرویس
            await _userService.AddService(user);

            return RedirectToAction("Index"); // یا هر اکشنی که جزئیات کاربر را نمایش می‌دهد
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(int provinceId)
        {
            // Fetch cities based on the provinceId
            var cities = await _cityService.GetCitiesByProvinceIdService(provinceId);

            // Assuming cities is a list of { Id, Name }
            var cityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.Name
            }).ToList();

            return Json(cityList); // Return JSON
        }
    }
}
