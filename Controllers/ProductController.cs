using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShopPet.Data;
using MyShopPet.Models.ViewModels.ProductViewModel;
using MyShopPet.Repositories.Abstraction;
using MyShopPet.Services.Abstraction;

namespace MyShopPet.Controllers
{
    [Authorize(Policy = "Admin policy")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Details(int id)
        {
            Product? product = await _productRepository.GetAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        public async Task<IActionResult> Create()
        {
            CreateProductViewModel vM = new()
            {
                  CategorySL = GetCategorySelectList(await _categoryRepository.GetAll().ToListAsync()),
             };
            return View(vM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel vM, [FromServices] IFileSavingHandler handler)
        {
            if (!ModelState.IsValid)
            {
                SelectList CategorySL = GetCategorySelectList(await _categoryRepository.GetAll().ToListAsync());
                vM.CategorySL = CategorySL;
                return View(vM);
            }
            Product product = _mapper.Map<Product>(vM);
            product.Photos = new();
            if (vM.Photos != null)
            {
                foreach (var photo in vM.Photos)
                {
                    string path = await SaveFile(handler, photo);
                    product.Photos.Add(new Photo { Filename = photo.FileName, PhotoUrl = path });
                }
            }
            await _productRepository.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }
        private async Task<string> SaveFile([FromServices] IFileSavingHandler handler, IFormFile photo)
        {
            string path = await handler.SaveFileAsync(photo);
            return path;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
                return NotFound();
            EditProductViewModel vM = new()
            {
                Product = product,
                CategorySL = GetCategorySelectList(await _categoryRepository.GetAll().ToListAsync()),
            };
            return View(vM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditProductViewModel vM)
        {
            if (id != vM.Product.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(vM.Product);
                return RedirectToAction("Index", "Admin");
            }
            SelectList CategorySL = GetCategorySelectList(await _categoryRepository.GetAll().ToListAsync());
            vM.CategorySL = CategorySL;
            return View(vM);
        }
      
        public async Task<IActionResult> Delete(int id)
        {
            Product? product = await _productRepository.GetAsync(id);
            if (product == null)
                return NotFound();
            DeleteProductViewModel vM = new()
            {
                Product = product
            };
            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteProductViewModel vM)
        {
            await _productRepository.DeleteAsync(vM.Product);
            return RedirectToAction("Index");
        }
        private SelectList GetCategorySelectList(List<Category> categories)
        {
            return new SelectList(categories, nameof(Product.Id), nameof(Product.Title));
        }
    }
}
