using MyShopPet.Models.ViewModels.AdminViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShopPet.Data;
using MyShopPet.Interfaces;

namespace MyShopPet.Controllers
{
    [Authorize(Policy = "Admin policy")]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<HomeController> _logger;

		public AdminController(ILogger<HomeController> logger, 
            IProductRepository productRepository, 
            IWebHostEnvironment environment,
            ICategoryRepository categoryRepository,
            IPhotoRepository photoRepository)
        {
            _productRepository = productRepository;
            _environment = environment;
            _logger = logger;
            _categoryRepository = categoryRepository;
            _photoRepository = photoRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            AdminIndexViewModel vM = new()
            {
                Products = products!
            };
            return View(vM);
        }

        public async Task<IActionResult> Details(int id)
        {
            Product? product = await _productRepository.GetAsync(id);
            if (product == null)
                return NotFound();  
            return View(product);
        }
		public IActionResult IsCategoryTitleAvailable(string title)
        {
            return Json(!_categoryRepository.IsExist(title));
        }

		public IActionResult CreateCategory()
        {
            CreateCategoryViewModel vM = new()
            {
                 ParentCategorySL = new SelectList(_categoryRepository.GetAll(), nameof(Category.Id), nameof(Category.Title))
            };
            return View(vM);
        }

        public IActionResult CreateProduct()
        {
            CreateProductViewModel vM = new()
            {
                CategorySL = new SelectList(_categoryRepository.GetAll(), nameof(Product.Id), nameof(Product.Title))
            };
            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel vM)
        {
            if (!ModelState.IsValid)
            {
                SelectList ParentCategorySL = new(_categoryRepository.GetAll(), nameof(Category.Id), nameof(Category.Title));
                vM.ParentCategorySL = ParentCategorySL;
                return View(vM);
            }
            Category category = new() 
            { 
                Title = vM.Title,
                ParentCategoryId = vM.ParentCategoryId 
            };
            await _categoryRepository.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel vM)
        {
            if (!ModelState.IsValid)
            {
                SelectList CategorySL = new(_categoryRepository.GetAll(), nameof(Product.Id), nameof(Product.Title));
                vM.CategorySL = CategorySL;
                return View(vM);
            }
            Product product = new()
            {
                Title = vM.Title,
                Count = vM.Count,
                Price = vM.Price,
                CategoryId = vM.CategoryId
            };
            product.Photos = new List<Photo>();
            if (vM.Photos != null)
            {
                foreach (var photo in vM.Photos)
                {
                    string root = $"{Path.GetFileNameWithoutExtension(photo.FileName)}" +
                        $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                    root = $"/images/{root}";
                    string fileFullPath = _environment.WebRootPath + root;
                    using FileStream fs = new(fileFullPath, FileMode.Create, FileAccess.Write);
                    photo.CopyTo(fs);
                    product!.Photos!.Add(new Photo { Filename = photo.FileName, PhotoUrl = root });
                }
            }
            await _productRepository.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
                return NotFound();
            EditProductViewModel vM = new()
            {
                Product = product,
                CategorySL = new(_categoryRepository.GetAll(), nameof(Product.Id), nameof(Product.Title)),    
            };
            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, EditProductViewModel vM)
        {
            if (id != vM.Product.Id)
                return NotFound();
            if (ModelState.IsValid)
            {       
                    await _productRepository.UpdateAsync(vM.Product);
                    return RedirectToAction("Index", "Admin");
            }
            SelectList CategorySL = new(_categoryRepository.GetAll(), nameof(Product.Id), nameof(Product.Title), vM.Product.Category);
            vM.CategorySL = CategorySL;
            return View(vM);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Photo>>> UploadImages(int id)
        {
            var files = Request.Form.Files;
            List<Photo> photos = new List<Photo>();
            foreach(var photo in files)
            {
                string root = $"{Path.GetFileNameWithoutExtension(photo.FileName)}" +
                        $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                root = $"/images/{root}";
                string fileFullPath = _environment.WebRootPath + root;
                using (FileStream fs = new(fileFullPath, FileMode.Create, FileAccess.Write))
                {
                    photo.CopyTo(fs);
                    Photo dbPhoto = new Photo { Filename = photo.FileName, PhotoUrl = root, ProductId = id };
                    await _photoRepository.CreateAsync(dbPhoto);
                    photos.Add(dbPhoto);
                }
            }
            return Ok(photos);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int id)
        {
            Photo? photo = await _photoRepository.GetAsync(id);
            if (photo == null)
                return NotFound();
            await _photoRepository.DeleteAsync(photo);
            return Ok();
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? product = await _productRepository.GetAsync(id);
            if (product == null)
                return NotFound();
            DeleteProductViewModel vM = new()
            {
                Product = product!
            };
            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(DeleteProductViewModel vM)
        {
            await _productRepository.DeleteAsync(vM.Product);
            return RedirectToAction("Index");
        }
    }
}
