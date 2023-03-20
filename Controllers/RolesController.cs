using System.Xml.Linq;
using MyShopPet.Models.ViewModels.RolesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyShopPet.Controllers
{
    [Authorize(Policy = "Admin policy")]
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityRole newRole = new(name);
                IdentityResult result = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            ModelState.AddModelError(string.Empty, "Role name can't be empty!");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if(role != null)
               await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        public async Task<IActionResult> ChangeUserRoles(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            ChangeRolesViewModel vM = new()
            {
                UserId = user.Id,
                AllRoles = allRoles,
                UserRoles = userRoles,
                UserName = user.UserName
            };
            return View(vM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangeUserRoles(string? id, List<string> roles)
        {
            if (id == null)
                return NotFound();
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null) 
                return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            IEnumerable<string> addedRoles = roles.Except(userRoles);
            IEnumerable<string> deletedRoles = userRoles.Except(roles);
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, deletedRoles);
            return RedirectToAction("UserList");
        }
	}
}
