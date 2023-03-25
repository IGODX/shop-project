using System.ComponentModel.DataAnnotations;
using MyShopPet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyShopPet.Models.ViewModels.ProductViewModel
{
    public class CreateProductViewModel
    {
        [Required]
        [MaxLength(60, ErrorMessage = "Too long title!")]
        public string Title { get; set; } = default!;
        [Required]
        public double Price { get; set; }
        [Required]
        public int Count { get; set; }
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public SelectList? CategorySL { get; set; } = default!;

        public IEnumerable<IFormFile> Photos { get; set; } = default!;
    }
}
