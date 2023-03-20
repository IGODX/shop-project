using System.ComponentModel.DataAnnotations;

namespace MyShopPet.Models.DTOs.UserDTOs
{
    public class UserDTO
    {
        [Required]
        public string Id { get; set; } = default!;
        [Required]
        public string UserName { get; set; } = default!;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
    }
}
