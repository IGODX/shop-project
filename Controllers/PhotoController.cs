using Microsoft.AspNetCore.Mvc;
using MyShopPet.Data;
using MyShopPet.Repositories.Abstraction;
using MyShopPet.Services.Abstraction;

namespace MyShopPet.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IFileSavingHandler _handler;

        public PhotoController(IPhotoRepository photoRepository, IFileSavingHandler handler)
        {
            _photoRepository = photoRepository;
            _handler = handler;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Photo>>> UploadImages(int id)
        {
            var files = Request.Form.Files;
            List<Photo> photos = new List<Photo>();
            foreach (var photo in files)
            {
                string path = await SaveFile(photo);
                Photo dbPhoto = new Photo { Filename = photo.FileName, PhotoUrl = path, ProductId = id };
                await _photoRepository.CreateAsync(dbPhoto);
                photos.Add(dbPhoto);
            }
            return Ok(photos);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int id)
        {
            Photo? photo = await _photoRepository.GetAsync(id);
            if (photo == null)
                return NotFound();
            await _photoRepository.DeleteAsync(photo);
            return Ok();
        }
        private async Task<string> SaveFile(IFormFile photo)
        {
            string path = await _handler.SaveFileAsync(photo);
            return path;
        }
    }
}
