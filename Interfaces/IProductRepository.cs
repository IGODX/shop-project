using MyShopPet.Data;

namespace MyShopPet.Interfaces
{
    public interface IProductRepository : IRepository<Product>, IUpdatable<Product>
    {

    }
}
