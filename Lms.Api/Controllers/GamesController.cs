using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using Lms.Data.Repositories;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        //private readonly LmsApiContext _context;
        private readonly UoW uow;

        public GamesController(LmsApiContext context)
        {
            //_context = context;
            uow = new UoW(context);

        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            var games = await uow.GameRepository.GetAllAsync();
            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
          if (uow.GameRepository == null)
          {
              return NotFound();
          }
            var game = await uow.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //public async Task<IActionResult> PutGame(int id, Game game)
        //{
        //    if (id != game.GameId)
        //    {
        //        return BadRequest();
        //    }

        //    uow.Entry(game).State = EntityState.Modified;

        //    try
        //    {
        //        await uow.CompleteAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GameExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
          if (uow.GameRepository == null)
          {
              return Problem("Entity set 'LmsApiContext.Game'  is null.");
          }
            uow.GameRepository.Add(game);
            await uow.CompleteAsync();

            return CreatedAtAction("GetGame", new { id = game.GameId }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (uow.GameRepository == null)
            {
                return NotFound();
            }
            var game = await uow.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            uow.GameRepository.Remove(game);
            await uow.CompleteAsync();

            return NoContent();
        }

        //private bool GameExists(int id)
        //{
        //    return (uow.GameRepository?.Any(e => e.GameId == id)).GetValueOrDefault();
        //}
    }
}
