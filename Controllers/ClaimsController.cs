using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyShopPet.Controllers
{
    [Authorize(Policy = "Admin policy")]
    public class ClaimsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ClaimsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(User.Claims);
        }

        public IActionResult Create()
        {
            return View();        
        }


        [HttpPost]
        public async Task<IActionResult> Create(string claimType, string claimValue)
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)   
            {
                Claim claim = new(claimType, claimValue, ClaimValueTypes.String);
                var result = await _userManager.AddClaimAsync(user, claim);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    Errors(result);
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public void Errors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string claimsInfo)
        {

            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return RedirectToAction("Login", "Account");
			string[] info = claimsInfo.Split(';');
            IEnumerable<Claim> claims = await _userManager.GetClaimsAsync(user);
            Claim? claimForDelete = claims.FirstOrDefault(e => e.Type == info[0] && e.Value == info[1] && e.ValueType == info[2]);
            if (claimForDelete != null)
                await _userManager.RemoveClaimAsync(user, claimForDelete);
            return RedirectToAction("Index");

		}
		[Authorize(Roles = "admin")]
		//[Authorize(Policy ="FrameworkPolicy")]
		public IActionResult TestPolicy1() => Content("Policy");
	}
}
