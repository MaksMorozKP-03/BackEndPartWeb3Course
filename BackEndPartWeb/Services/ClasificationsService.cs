using BackEndPartWeb.Data;
using BackEndPartWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndPartWeb.Services
{
    public interface IClasificationsService
    {
        Task<IEnumerable<Clasification>> GetAllClasifications();
        Task<Clasification> GetClasificationByName(string name);
    }
    public class ClasificationsService: IClasificationsService
    {
        private BestiaryContext _context;
        public ClasificationsService(BestiaryContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Clasification>> GetAllClasifications()
        {
            var clasifications = await _context.Clasifications.ToListAsync();
            return clasifications;
        }

        public async Task<Clasification> GetClasificationByName(string name)
        {
            var result = await _context.Clasifications.FirstOrDefaultAsync(c=> c.Name == name);
            return result;
        }
    }
}
