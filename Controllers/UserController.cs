using MyShopPet.Models.DTOs.UserDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyShopPet.Controllers
{
    [Authorize(Policy = "Admin policy")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<IdentityUser> userManager, IMapper mapper)
        {
			this.userManager = userManager;
            _mapper = mapper;
        }
        public IActionResult Create()
        {
            return View();   
        }
        public IActionResult Index()
        {
            var users = userManager.Users;
            //user auto-mapper in future!
            IEnumerable<UserDTO> usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            return View(usersDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = _mapper.Map<IdentityUser>(dto);
                var result = await userManager.CreateAsync(user, dto.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);	
            }
            return View(dto);
        }

        public IActionResult IsEmailAvailable(string email)
        {
            return Json(!userManager.Users.Select(e => e.Email).Any(e => e == email));
		}
		public IActionResult IsUserNameAvailable(string userName)
		{
			return Json(!userManager.Users.Select(e => e.UserName).Any(e => e == userName));
		}

		public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
                return NotFound();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            //Auto-mapper
            EditUserDTO dto = _mapper.Map<EditUserDTO>(user);
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserDTO dto)
        {
             if(ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(dto.Id);
                if (user == null)
                    return NotFound();
                user.Email = dto.Email;
                user.UserName = dto.UserName;
                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

        }
            return View(dto);
        }
        public async Task<IActionResult> ChangePassword(string? id)
        {
            if (id == null)
                return NotFound();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            ChangePasswordDTO dto = _mapper.Map<ChangePasswordDTO>(user);
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByIdAsync(dto.Id);
                var passwordValidator = HttpContext.RequestServices.GetRequiredService<IPasswordValidator<IdentityUser>>();
                var passwordHasher = HttpContext.RequestServices.GetRequiredService<IPasswordHasher<IdentityUser>>();
                if (user == null)
                    return NotFound();
                var result = await passwordValidator.ValidateAsync(userManager, user, dto.NewPassword);
                if (result.Succeeded)
                {
                    string hashedPassword = passwordHasher.HashPassword(user, dto.NewPassword);
                    user.PasswordHash = hashedPassword;
                    await userManager.UpdateAsync(user);
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(dto);
        }
        public async Task<IActionResult>Delete(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            await userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}
