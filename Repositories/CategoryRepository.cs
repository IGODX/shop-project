using Microsoft.EntityFrameworkCore;
using MyShopPet.Data;
using MyShopPet.Repositories.Abstraction;

namespace MyShopPet.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ShopDbContext _context;

        public CategoryRepository(ShopDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            _context.Categories.Remove(category!);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetAsync(int id)
        {
            return await _context.Categories.Include(e=> e.ChildCategories).FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories.Include(e=> e.ChildCategories);
        }

        public bool IsExist(string title)
        {
            return _context.Categories.Select(e => e.Title).Any(e => e == title);
        }

        public async Task DeleteAsync(Category entity)
        {
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
             await _context.SaveChangesAsync();
        }
    }
}
