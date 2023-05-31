using BackEndPartWeb.Data;
using BackEndPartWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndPartWeb.Services
{
    public interface IRolesService
    {
        Task<IEnumerable<Role>> GetAllRoles();
    }
    public class RolesService : IRolesService
    {
        private BestiaryContext _context;
        public RolesService(BestiaryContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles;
        }      
    }
}
