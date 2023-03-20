using Microsoft.AspNetCore.Identity;

namespace MyShopPet.Models.ViewModels.RolesViewModels
{
    public class ChangeRolesViewModel
    {
        public string UserId { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public IList<string> UserRoles { get; set; } = default!;

        public List<IdentityRole> AllRoles { get; set; } = default!;
    }
}
