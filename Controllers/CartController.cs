using MyShopPet.Data;
using MyShopPet.Extensions;
using MyShopPet.Models.ViewModels.CartViewModel;
using Microsoft.AspNetCore.Mvc;
using MyShopPet.Interfaces;
using MyShopPet.Models.Domain;

namespace MyShopPet.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        //public IActionResult Index(string? returnUrl)
        public IActionResult Index(Cart cart, string? returnUrl)
        {
            //Cart cart = GetCart();
            if (returnUrl == null)
                returnUrl = Url.Action("Index", "Home");
            CartIndexViewModel vM = new()
            {
                Cart = cart,
                ReturnUrl = returnUrl
            };
            return View(vM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(Cart cart, int id, string? returnUrl)
        {
            Product? product = await _productRepository.GetAsync(id);
            if (product != null)
            {
                cart.AddToCard(product, 1);
                HttpContext.Session.Set("cart", cart.CartItems);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public async Task<IActionResult> DeleteFromCart(Cart cart, int id, string? returnUrl)
        {
            //Cart cart = GetCart();
            Product? product = await _productRepository.GetAsync(id);
            if (product != null)
            {
                cart.RemoveFromCart(product);
                HttpContext.Session.Set("cart", cart.CartItems);
            }
            return RedirectToAction("Index", new { returnUrl});
        }
    }
}
