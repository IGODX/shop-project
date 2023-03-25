using MyShopPet.Data;

namespace MyShopPet.Repositories.Abstraction
{
    public interface ICategoryRepository : IRepository<Category>, IUpdatable<Category>
    {
        public bool IsExist(string title);
    }
}
