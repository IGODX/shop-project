namespace MyShopPet.Repositories.Abstraction
{
    public interface IRepository<T>
    {
        public Task CreateAsync(T entity);
        public Task<T?> GetAsync(int id);
        public IQueryable<T> GetAll();
        public Task DeleteAsync(int id);
        public Task DeleteAsync(T entity);
    }
}
