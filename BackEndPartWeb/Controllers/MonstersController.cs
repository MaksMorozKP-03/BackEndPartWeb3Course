using BackEndPartWeb.Models;
using BackEndPartWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BackEndPartWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonstersController : ControllerBase
    {
        private IMonstersService _monstersService;

        public MonstersController(IMonstersService monstersService)
        {
            _monstersService = monstersService;
        }
        [HttpGet]
        public async Task<IEnumerable<Monster>> GetAllMonsters()
        {
            return await _monstersService.GetMonstersAsync();
        }
        [HttpGet("{id}")]
        public async Task<Monster> GetMonster(Guid id)
        {
            return await _monstersService.GetMonsterById(id);
        }
        [HttpPost]
        public async Task<ActionResult<Monster>> AddMonster(Monster monster)
        {
            try
            {
                var result = await _monstersService.AddMonsterAsync(monster);
                return Ok(result);
            }
            catch (ValidationException ex)
            { return BadRequest(ex.Message); }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Monster>> DeleteMonster(Guid id)
        {
            try
            {
                var result = await _monstersService.DeleteMonsterAsync(id);
                return Ok(result);
            }
            catch (ArgumentNullException ex) { return BadRequest(ex.Message); }

        }
        [HttpPatch]
        public async Task<ActionResult<Monster>> UpdateMonster(Monster newMonster)
        {
            try
            {
                var result = await _monstersService.UpdateMonsterAsync(newMonster);
                return Ok(result);
            }
            catch (ArgumentNullException ex) { return BadRequest(ex.Message); }
        }
    }
}
