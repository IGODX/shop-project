using MyShopPet.Data;

namespace MyShopPet.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>, IUpdatable<Category>
    {
        public bool IsExist(string title);
    }
}
