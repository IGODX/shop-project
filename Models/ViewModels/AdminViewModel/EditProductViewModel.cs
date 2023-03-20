using MyShopPet.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyShopPet.Models.ViewModels.AdminViewModel
{
    public class EditProductViewModel
    {
        public Product Product { get; set; } = default!;

        public SelectList? CategorySL { get; set; }
    }
}
