using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StructureApp.Models.ViewModels;

namespace StructureApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            User user = new User();
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Email;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return View(model);
            
        }
        // Login
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByNameAsync(model.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "کاربر یافت نشد");
                return View(model);
            }
            await _signInManager.SignOutAsync();

            var result = await _signInManager.PasswordSignInAsync(user, model.Password , model.IsPersistent , true);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl);
            }
            if(result.RequiresTwoFactor)
            {
                // 
            }
            return View();
        }
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // Profile
        public async Task<IActionResult> Profile()
        {
            return View();
        }
    }
}
