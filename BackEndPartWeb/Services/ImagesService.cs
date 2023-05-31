using BackEndPartWeb.Data;
using BackEndPartWeb.Models;
using Microsoft.IdentityModel.Tokens;

namespace BackEndPartWeb.Services
{
    public interface IImagesService
    {
        Task<Image> GetImageAsync(Guid id);
        Task<bool> AddImageAsync(Image image);
        Task<bool> RemoveImage(Image image);
        Task<bool> UpdateImage(Guid id, Image request);
    }
    public class ImagesService : IImagesService
    {
        private BestiaryContext _context;
        public ImagesService(BestiaryContext context)
        {
            _context = context;
        }
        public async Task<Image> GetImageAsync(Guid id)
        {
            var image = await _context.Images.FindAsync(id);
            return image;
        }
        public async Task<bool> AddImageAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveImage(Image image)
        {
            _context.Images.Remove(image);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch { return false; }
            return true;
        }
        public async Task<bool> UpdateImage(Guid id, Image request)
        {
            var image = _context.Images.Find(id);
            if (image == null) return false;
            image.Name = request.Name;
            image.Content = request.Content;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
