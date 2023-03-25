using Microsoft.EntityFrameworkCore;
using MyShopPet.Data;
using MyShopPet.Repositories.Abstraction;

namespace MyShopPet.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopDbContext _context;
        public ProductRepository(ShopDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            _context.Products.Remove(product!);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetAsync(int id)
        {
            return await _context.Products.
                Include(e=> e.Photos).
                Include(e=> e.Category).
                FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products.
                Include(e => e.Category).
                Include(e => e.Photos);
        }

        public async Task DeleteAsync(Product entity)
        {
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
