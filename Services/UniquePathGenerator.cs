using MyShopPet.Services.Abstraction;

namespace MyShopPet.Services
{
    public class UniquePathGenerator : IUniquePathGenerator
    {
        private readonly IWebHostEnvironment _environment;
        public UniquePathGenerator(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public (string,string) GeneratePath(IFormFile photo)
        {
            string root = $"{Path.GetFileNameWithoutExtension(photo.FileName)}" +
            $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            root = $"/images/{root}";
            return (root, _environment.WebRootPath + root);
        }
    }
}
