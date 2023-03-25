namespace MyShopPet.Services.Abstraction
{
    public interface IFileSavingHandler
    {
        public Task<string> SaveFileAsync(IFormFile photo);
    }
}
