using MyShopPet.Models.Domain;

namespace MyShopPet.Models.ViewModels.CartViewModel
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; } = default!;
        public string? ReturnUrl { get; set; }
    }
}
