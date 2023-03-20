using Microsoft.EntityFrameworkCore;
using MyShopPet.Data;
using MyShopPet.Interfaces;

namespace MyShopPet.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly ShopDbContext _context;
        public PhotoRepository(ShopDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Photo entity)
        {
            await _context.Photos.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Photo? Photo = await _context.Photos.FirstOrDefaultAsync(c => c.Id == id);
            _context.Photos.Remove(Photo!);
            await _context.SaveChangesAsync();
        }

        public async Task<Photo?> GetAsync(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Photo> GetAll()
        {
            return _context.Photos;
        }

        public async Task DeleteAsync(Photo entity)
        {
            _context.Photos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
