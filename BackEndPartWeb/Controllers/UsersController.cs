using BackEndPartWeb.Models;
using BackEndPartWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BackEndPartWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _usersService.GetUsersAsync();
        }
        [HttpGet("{id}")]
        public async Task<User> GetUser(Guid id)
        {
            return await _usersService.GetUserById(id);
        }
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            user.Password = new PasswordHasher<User>().HashPassword(user, user.Password);
            try
            {
                var result = await _usersService.AddUserAsync(user);
                return Ok(result);
            }
            catch (ValidationException ex)
            { return BadRequest(ex.Message); }


        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            try
            {
                var result = await _usersService.DeleteUserAsync(id);
                return Ok(result);
            }
            catch (ArgumentNullException ex) { return BadRequest(ex.Message); }
        }
        [HttpPatch]
        public async Task<ActionResult<User>> UpdateUser(User newUser)
        {
            try
            {
                var result = await _usersService.UpdateUserAsync(newUser);
                return Ok(result);
            }
            catch (ArgumentNullException ex) { return BadRequest(ex.Message); }

        }
        [HttpGet]
        [Route("subscriptions/{id}")]
        public async Task<ActionResult<IEnumerable<Monster>>> GetUsersMonsters(Guid id)
        {
            try
            {
                var monsters = await _usersService.GetUsersMonstersAsync(id);
                return Ok(monsters);
            }
            catch (ArgumentNullException ex) { return BadRequest(ex.Message); }
            
        }
        [HttpPatch]
        [Route("subscriptions/add/{userId}&{monsterId}")]
        public async Task<ActionResult<IEnumerable<Monster>>> AddMonsterToUser(Guid userId, Guid monsterId)
        {
            try
            {
                var result = await _usersService.AddMonsterToUserAsync(userId, monsterId);
                return Ok(result);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }
        [HttpPatch]
        [Route("subscriptions/remove/{userId}&{monsterId}")]
        public async Task<ActionResult<IEnumerable<Monster>>> RemoveMonsterFromUser(Guid userId, Guid monsterId)
        {

            try
            {
                var result = await _usersService.RemoveMonsterFromUserAsync(userId, monsterId);
                return Ok(result);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            
        }
    }
}
