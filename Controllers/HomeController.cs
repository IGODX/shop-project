using System.Diagnostics;
using MyShopPet.Models.ViewModels.HomeViewModel;
using Microsoft.AspNetCore.Mvc;
using MyShopPet.Data;
using MyShopPet.Interfaces;
using MyShopPet.Models;

namespace MyShopPet.Controllers
{
    public class HomeController : Controller
    {
		private readonly IProductRepository _productRepository;

		public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index(string? category, int page = 1)
        {
            if (page == 0)
                return NotFound();
            IQueryable<Product> products = _productRepository.GetAll();
            if (category != null)
                products = products.Where(e => e.Category!.Title == category);
            const int ItemsPerPage = 10;
            int pageCount = (int)Math.Ceiling((decimal)products.Count() / ItemsPerPage);
            products = products.Skip((page - 1) * ItemsPerPage).Take(ItemsPerPage);
            HomeIndexViewModel vM = new()
            {
                Products = products,
                Category = category,
                Page = page,
                PageCount = pageCount
            };
            return View(vM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}