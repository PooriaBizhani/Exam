using First_Sample.Application.InterFaces;
using First_Sample.Application.Logs;
using First_Sample.Domain.ViewModels.Login;
using First_Sample.Domain.ViewModels.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace First_Sample.Presentation.Controllers
{
    [ServiceFilter(typeof(LogActionFilter))]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMessageSenderService _messageSenderService;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMessageSenderService messageSenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageSenderService = messageSenderService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var emailMessage = Url.Action("ConfirmEmail", "Account",
                    new { username = user.UserName, token = emailConfirmationToken },
                    Request.Scheme);

                await _messageSenderService.SendEmailService(model.Email, "Email Confirmation", emailMessage);

                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                // گرفتن UserId و UserName بعد از ورود موفق
                var user = await _userManager.FindByNameAsync(model.UserName);
                var userId = user?.Id;
                var userName = user?.UserName;

                // ثبت لاگ ورود کاربر در Serilog
                Log.ForContext("UserId", userId)
                   .ForContext("UserName", userName)
                   .ForContext("Action", "Login")
                   .ForContext("Controller", "Account")
                   .Information($"User {userName} with ID {userId} has logged in.");

                if (string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ViewData["ErrorMessage"] = "5 min lock";
            }

            ModelState.AddModelError("", "Invalid Data");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var usuer = await _userManager.FindByEmailAsync(email);
            if (usuer == null) return Json(true);
            return Json("Email Is Not Valid");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUserNameInUse(string userName)
        {
            var usuer = await _userManager.FindByNameAsync(userName);
            if (usuer == null) return Json(true);
            return Json("UserName Is Not Valid");
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return NotFound();

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");
        }

        //[HttpGet]
        //public async IActionResult ChangePassword()
        //{
        //    await _userManager.
        //    return View();
        //}
    }
}
