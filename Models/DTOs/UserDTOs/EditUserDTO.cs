using System.ComponentModel.DataAnnotations;

namespace MyShopPet.Models.DTOs.UserDTOs
{
    public class EditUserDTO
    {
        [Required]
        public string Id { get; set; } = default!;
        [Required]
        public string UserName { get; set; } = default!;

        [Required]
        [Display(Name = "Date of birth")]
        public int YearOfBirth { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
    }
}
