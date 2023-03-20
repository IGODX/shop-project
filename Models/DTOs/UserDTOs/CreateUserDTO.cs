using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyShopPet.Models.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        [Required]
		[Remote("IsUserNameAvailable", "User", HttpMethod = "POST", ErrorMessage = "Username is already exist!")]
		public string UserName { get; set; } = default!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote("IsEmailAvailable", "User", HttpMethod = "POST", ErrorMessage = "Email is already exist!")]
        public string Email { get; set; } = default!;

    }
}
