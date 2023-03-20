using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShopPet.Data;

namespace MyShopPet.Models.ViewModels.AdminViewModel
{
    public class CreateCategoryViewModel
    {
		[Required]
		[MaxLength(60, ErrorMessage = "Too long title!")]
		[Remote("IsCategoryTitleAvailable", "Admin", HttpMethod = "POST", ErrorMessage = "Category is already exist!")]
		public string Title { get; set; } = default!;
		public int? ParentCategoryId { get; set; }
		[Display(Name = "Parent category")]
		public Category? ParentCategory { get; set; }
		public SelectList? ParentCategorySL { get; set; }
    }
}
