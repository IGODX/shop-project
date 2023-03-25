using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyShopPet.Models.ViewModels.AccountViewModel;

namespace MyShopPet.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
			this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vM)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new()
                {
                    Email = vM.Email,
                    UserName = vM.Login,
                };
                var result = await userManager.CreateAsync(user, vM.Password);
				if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(vM);
        }
		public IActionResult IsLoginAvailable(string login)
		{
			return Json(!userManager.Users.Select(e => e.UserName).Any(e => e == login));
		}
		public IActionResult IsEmailAvailable(string email)
		{
			return Json(!userManager.Users.Select(e => e.Email).Any(e => e == email));
		}
		public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel vM = new()
            {
                ReturnUrl = returnUrl
            };
            return View(vM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vM)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(vM.Login,
                    vM.Password, vM.IsPersistent, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(vM.ReturnUrl) && Url.IsLocalUrl(vM.ReturnUrl))
                    {
                        return Redirect(vM.ReturnUrl);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty,"Login or Password wrong!");

            }
            return View(vM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult GoogleAuth()
        {
            string? redirectURL = Url.Action("GoogleRedirect", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectURL);
            return new ChallengeResult("Google", properties);
        }
        [AllowAnonymous]
        public IActionResult FbAuth()
        {
            string? redirectURL = Url.Action("GoogleRedirect", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectURL);
            return new ChallengeResult("Facebook",properties);
        }

        public async Task<IActionResult> GoogleRedirect()
        {
            ExternalLoginInfo loginInfo = await signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
                return RedirectToAction("Login");
            var loginResult = await signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);
            string?[] userInfo =
            {
                loginInfo.Principal.FindFirst(ClaimTypes.Name)?.Value,
                loginInfo.Principal.FindFirst(ClaimTypes.Email)?.Value,
            };
            if (loginResult.Succeeded)
                return View(userInfo);
            IdentityUser user = new()
            {
                UserName = userInfo[1],
                Email = userInfo[1]
            };
            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                    result = await userManager.AddLoginAsync(user, loginInfo);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return View(userInfo);
                }
            }
            else
            {
                    IdentityUser? findedUser = await userManager.FindByEmailAsync(userInfo[1]);
                    if(findedUser != null)
					await userManager.AddLoginAsync(findedUser!, loginInfo);
			}
            return RedirectToAction(nameof(AccessDenied));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
