using MyShopPet.Data;

namespace MyShopPet.Models.ViewModels.HomeViewModel
{
	public class HomeIndexViewModel
	{
		public IEnumerable<Product> Products { get; set; } = default!;

		public string? Category { get; set; }

        public int Page { get; set; }
        public int PageCount { get; set; }
    }
}
