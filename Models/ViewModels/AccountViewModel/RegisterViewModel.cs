using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyShopPet.Models.ViewModels.AccountViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Login")]
		[Remote("IsLoginAvailable", "Account", HttpMethod = "POST", ErrorMessage = "Login is already exist!")]
		public string Login { get; set; } = default!;
        [Required]
        [Display(Name = "Email address")]
        [DataType(DataType.EmailAddress)]
		[Remote("IsEmailAvailable", "Account", HttpMethod = "POST", ErrorMessage = "Email is already exist!")]
		public string Email { get; set; } = default!;
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The passwords must match!")]
        public string ConfirmPassword { get; set; } = default!;

    }
}
