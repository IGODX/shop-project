namespace MyShopPet.Repositories.Abstraction
{
    public interface IUpdatable<T> where T : class
    {
        public Task UpdateAsync(T entity);
    }
}
