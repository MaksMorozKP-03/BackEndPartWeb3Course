using BackEndPartWeb.Data;
using BackEndPartWeb.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BackEndPartWeb.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserById(Guid id);
        Task<User> AddUserAsync(User user);
        Task<User> DeleteUserAsync(Guid id);
        Task<User> UpdateUserAsync(User request);
        Task<IEnumerable<Monster>> GetUsersMonstersAsync(Guid userId);
        Task<IEnumerable<Monster>> AddMonsterToUserAsync(Guid userId, Guid monsterId);
        Task<IEnumerable<Monster>> RemoveMonsterFromUserAsync(Guid userId, Guid monsterId);
    }
    public class UsersService : IUsersService
    {
        private BestiaryContext _context;
        public UsersService(BestiaryContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                .ToListAsync();
            return users;
        }
        public async Task<User> GetUserById(Guid id)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                .Include(u => u.Monsters)
                .SingleOrDefault(u => u.Id == id);            
            return user;
        }
        public async Task<User> AddUserAsync(User user)
        {
            ValidateUser(user);
            if (user.Image.Id != Guid.Empty) _context.Attach(user.Image);
            _context.Attach(user.Role);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        public async Task<User> DeleteUserAsync(Guid id)
        {
            var user = await this.GetUserById(id);
            if (user == null) throw new ArgumentNullException("","User does not exist");
            _context.Users.Remove(user);
            _context.Images.Remove(user.Image);
            _context.SaveChanges();
            return user;
        }
        public async Task<User> UpdateUserAsync(User request)
        {
            var user = await this.GetUserById(request.Id);
            if (user is null) throw new ArgumentNullException("", "User does not exist");
            if (request.Age != 0) user.Age = request.Age;
            if (!request.Username.Equals(user.Username)) user.Username = request.Username;
            if (!request.Email.Equals(user.Email)) user.Email = request.Email;
            if (!request.FullName.Equals(user.FullName)) user.FullName = request.FullName;
            if (request.Image is not null && !request.Image.Equals(user.Image)) {
                _context.Images.Remove(user.Image);
                user.Image = request.Image;
            } 
            else _context.Attach(user.Image);
            _context.Attach(user.Role);
            await _context.SaveChangesAsync(); 
            return user;
        }
        public async Task<IEnumerable<Monster>> GetUsersMonstersAsync(Guid id)
        {
            User user = _context.Users
                .Include(u => u.Monsters)
                .SingleOrDefault(u => u.Id == id);
            if (user == null) throw new ArgumentNullException("", "User does not exist");
            return user.Monsters;
        }

        
        private void ValidateUser(User user)
        {
            if (user.Age < 0) throw new ValidationException("Age must be begger then zero.");

            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            if (!Regex.IsMatch(user.Email, regex)) throw new ValidationException("Invalid email.");

            if (HasUsername(user.Username)) throw new ValidationException("Not unique username.");


        }
        private bool HasUsername(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            return user != null;
        }

        public async Task<IEnumerable<Monster>> AddMonsterToUserAsync(Guid userId, Guid monsterId)
        {
            var monster = await _context.Monsters.FirstOrDefaultAsync(m => m.Id == monsterId);
            var user = await GetUserById(userId);
            if (user == null) throw new ArgumentNullException("", "User does not exist");
            if (monster == null) throw new ArgumentNullException("", "Monster does not exist");
            if (user.Monsters.Exists(monster => monster.Id == monsterId)) throw new ArgumentException("User already has this monster");
            user.Monsters.Add(monster);
            await _context.SaveChangesAsync();
            return user.Monsters; 
        }

        public async Task<IEnumerable<Monster>> RemoveMonsterFromUserAsync(Guid userId, Guid monsterId)
        {
            var user = await GetUserById(userId);
            var monster = await _context.Monsters.FirstOrDefaultAsync(m => m.Id == monsterId);
            if (user == null) throw new ArgumentNullException("", "User does not exist");
            if (monster == null) throw new ArgumentNullException("", "Monster does not exist");
            if (!user.Monsters.Exists(monster => monster.Id == monsterId)) throw new ArgumentException("User already doesnt have this monster");
            user.Monsters.Remove(monster);
            await _context.SaveChangesAsync();
            return user.Monsters;
        }
    }
}
