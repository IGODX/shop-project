using MyShopPet.Data;

namespace MyShopPet.Repositories.Abstraction
{
    public interface IProductRepository : IRepository<Product>, IUpdatable<Product>
    {

    }
}
