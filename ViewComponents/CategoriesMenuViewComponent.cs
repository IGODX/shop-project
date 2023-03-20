using MyShopPet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopPet.Interfaces;

namespace MyShopPet.ViewComponents
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesMenuViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? currentCategory)
        {
            List<Category> categories = await _categoryRepository.GetAll().ToListAsync();
            return View(new Tuple<List<Category>, string?>(categories, currentCategory));
        }
    }
}
