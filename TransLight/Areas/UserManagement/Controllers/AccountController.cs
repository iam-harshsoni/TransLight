using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransLight.Common;
using TransLight.DataAccess.IdentityModel;
using TransLight.DataAccess.ViewModels.Account;
using TransLight.Services.Interfaces.Masters;

namespace TransLight.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyService _companyService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ICompanyService companyService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _companyService = companyService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginVM
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);


            if (result.Succeeded)
            {

                var companies = _companyService.GetAll();
                if (companies != null && companies.Any())
                {
                    var defaultCompany = companies.First();

                    HttpContext.Session.SetCompanyId(defaultCompany.Id);
                    HttpContext.Session.SetString("CompanyName", string.Concat(defaultCompany.Name, defaultCompany.Code));
                }

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return LocalRedirect(model.ReturnUrl);
                }

                // Default landing page
                return RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Dashboard" });
            }


            ModelState.AddModelError("", "Invalid login details");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account", new { area = "UserManagement" });
        }
    }
}
