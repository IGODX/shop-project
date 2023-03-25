using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShopPet.Data;
using MyShopPet.Models.ViewModels.CategoryViewModel;
using MyShopPet.Repositories.Abstraction;

namespace MyShopPet.Controllers
{
    [Authorize(Policy = "Admin policy")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IsCategoryTitleAvailable(string title)
        {
            return Json(!_categoryRepository.IsExist(title));
        }

        public async Task<IActionResult> CreateCategory()
        {
            CreateCategoryViewModel vM = new()
            {
                ParentCategorySL = new SelectList(await _categoryRepository.GetAll().ToListAsync())
            };
            return View(vM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel vM)
        {
            if (!ModelState.IsValid)
            {
                SelectList ParentCategorySL = new(await _categoryRepository.GetAll().ToListAsync());
                vM.ParentCategorySL = ParentCategorySL;
                return View(vM);
            }
            Category category = _mapper.Map<Category>(vM);
            await _categoryRepository.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }
        private SelectList GetCategorySelectList(List<Category> categories)
        {
            return new SelectList(categories, nameof(Product.Id), nameof(Product.Title));
        }
    }
}
