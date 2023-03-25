using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopPet.Models.ViewModels.AdminViewModel;
using MyShopPet.Repositories.Abstraction;

namespace MyShopPet.Controllers
{
    [Authorize(Policy = "Admin policy")]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;

        public AdminController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAll().ToListAsync();
            AdminIndexViewModel vM = new()
            {
                Products = products!
            };
            return View(vM);
        }
    }
}
