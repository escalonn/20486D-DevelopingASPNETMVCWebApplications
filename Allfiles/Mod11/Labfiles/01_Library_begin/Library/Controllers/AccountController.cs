using System.Security.Claims;
using System.Threading.Tasks;
using Library.Models;
using Library.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", controllerName: "Library");
            }
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                    userName: loginModel.UserName,
                    password: loginModel.Password,
                    isPersistent: loginModel.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", controllerName: "Library");
                }
            }

            ModelState.AddModelError(key: "", errorMessage: "Failed to Login");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", controllerName: "Library");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPost(
            RegisterViewModel registerModel,
            [FromServices] UserManager<User> userManager,
            [FromServices] RoleManager<IdentityRole> roleManager)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    PhoneNumber = registerModel.PhoneNumber,
                    Email = registerModel.Email,
                    UserName = registerModel.UserName,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName
                };

                IdentityResult result = await userManager.CreateAsync(user, password: registerModel.Password);

                if (result.Succeeded)
                {
                    if (!await userManager.IsInRoleAsync(user, registerModel.RoleName))
                    {
                        bool roleExists = await roleManager.RoleExistsAsync(registerModel.RoleName);

                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole(registerModel.RoleName));
                        }

                        await userManager.AddToRoleAsync(user, registerModel.RoleName);
                    }

                    if (!string.IsNullOrWhiteSpace(user.Email))
                    {
                        var claim = new Claim(ClaimTypes.Email, user.Email);
                        await userManager.AddClaimAsync(user, claim);
                    }

                    Microsoft.AspNetCore.Identity.SignInResult resultSignIn = await _signInManager.PasswordSignInAsync(
                        userName: registerModel.UserName,
                        password: registerModel.Password,
                        isPersistent: registerModel.RememberMe,
                        lockoutOnFailure: false);

                    if (resultSignIn.Succeeded)
                    {
                        return RedirectToAction("Index", controllerName: "Library");
                    }
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(key: "", errorMessage: error.Description);
                }
            }

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
