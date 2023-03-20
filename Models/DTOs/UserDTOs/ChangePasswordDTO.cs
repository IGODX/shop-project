using System.ComponentModel.DataAnnotations;

namespace MyShopPet.Models.DTOs.UserDTOs
{
    public class ChangePasswordDTO
    {
        public string Id { get; set; } = default!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;

    }
}
