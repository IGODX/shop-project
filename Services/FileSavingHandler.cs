using MyShopPet.Data;
using MyShopPet.Services.Abstraction;

namespace MyShopPet.Services
{
    public class FileSavingHandler : IFileSavingHandler
    {
        private readonly IUniquePathGenerator _pathGenerator;
        public FileSavingHandler(IUniquePathGenerator pathGenerator)
        {

            _pathGenerator = pathGenerator;

        }
        public async Task<string> SaveFileAsync(IFormFile photo)
        {
            var pathTuple = _pathGenerator.GeneratePath(photo);
            using FileStream fs = new(pathTuple.Item2, FileMode.Create, FileAccess.Write);
            await photo.CopyToAsync(fs);
            return pathTuple.Item1;
        }
    }
}
