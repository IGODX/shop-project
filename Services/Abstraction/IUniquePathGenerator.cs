namespace MyShopPet.Services.Abstraction
{
    public interface IUniquePathGenerator
    {
        public (string, string) GeneratePath(IFormFile photo);
    }
}
