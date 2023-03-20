using MyShopPet.Data;

namespace MyShopPet.Models.ViewModels.AdminViewModel
{
    public class AdminIndexViewModel
    {
        public IEnumerable<Product?>? Products { get; set; }
    }
}
