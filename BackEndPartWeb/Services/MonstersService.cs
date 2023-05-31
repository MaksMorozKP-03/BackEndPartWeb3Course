using BackEndPartWeb.Data;
using BackEndPartWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackEndPartWeb.Services
{
    public interface IMonstersService
    {
        Task<IEnumerable<Monster>> GetMonstersAsync();
        Task<Monster> GetMonsterById(Guid id);
        Task<Monster> AddMonsterAsync(Monster monster);
        Task<Monster> DeleteMonsterAsync(Guid id);
        Task<Monster> UpdateMonsterAsync(Monster request);
    }
    public class MonstersService : IMonstersService
    {
        private BestiaryContext _context;
        private IClasificationsService _clasificationsService;
        public MonstersService(BestiaryContext context, IClasificationsService clasificationsService)
        {
            _context = context;
            _clasificationsService = clasificationsService;
        }
        public async Task<IEnumerable<Monster>> GetMonstersAsync()
        {
            var monsters = await _context.Monsters
                .Include(m => m.Image)
                .Include(m => m.Clasification)
                .ToListAsync();
            return monsters;
        }
        public async Task<Monster> GetMonsterById(Guid id)
        {
            var monster = _context.Monsters
                .Include(m => m.Image)
                .Include(m => m.Clasification)
                .SingleOrDefault(m => m.Id == id);
            return monster;
        }
        public async Task<Monster> AddMonsterAsync(Monster monster)
        {
            ValidateMonster(monster);
            if (monster.Clasification.Id != Guid.Empty) _context.Attach(monster.Clasification);
            if (monster.Image.Id != Guid.Empty) _context.Attach(monster.Image);
            _context.Monsters.Add(monster);
            _context.SaveChanges();
            return monster;
        }
        public async Task<Monster> DeleteMonsterAsync(Guid id)
        {
            var monster = await GetMonsterById(id);
            if (monster is null) throw new ArgumentNullException("", "Monster does not exist");
            _context.Monsters.Remove(monster);
            _context.Images.Remove(monster.Image);
            _context.SaveChanges();
            return monster;
        }
        public async Task<Monster> UpdateMonsterAsync(Monster request)
        {
            var monster = await GetMonsterById(request.Id);
            if (monster == null) throw new ArgumentNullException("", "Monster does not exist");
            if (!request.Description.Equals(monster.Description)) monster.Description = request.Description;
            if (!request.Name.Equals(monster.Name)) monster.Name = request.Name;
            if (request.Image is not null && !request.Image.Equals(monster.Image)) monster.Image = request.Image;
            else _context.Attach(monster.Image);
            _context.Attach(monster.Clasification);
            await _context.SaveChangesAsync();
            return monster;
        }
        private void ValidateMonster(Monster monster)
        {

            if (HasName(monster.Name)) throw new ValidationException("Not unique name.");

            if (monster.Clasification.Id == Guid.Empty &&
                _clasificationsService.GetClasificationByName(monster.Clasification.Name) is not null)
                throw new ValidationException("Try to add not unique Clasification");
        }
        private bool HasName(string name)
        {
            var monster = _context.Monsters.FirstOrDefault(x => x.Name == name);
            return monster != null;
        }

    }
}
