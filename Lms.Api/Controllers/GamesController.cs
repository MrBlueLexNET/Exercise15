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
using Lms.Api.ResourceParameters;
using Lms.Core.DTOs;
using AutoMapper;

namespace Lms.Api.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly UoW uow;
        private readonly IMapper mapper;

        public GamesController(LmsApiContext context, IMapper mapper)
        {
            uow = new UoW(context);
            this.mapper = mapper;
        }

        //// GET: api/Games
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        //{
        //    var games = await uow.GameRepository.GetAllAsync();
        //    return Ok(games);
        //}

        [HttpGet]
        [HttpHead]
        //Search and Filter || ?searchQuery && ?name
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(
        [FromQuery] GamesResourceParameters gamesResourceParameters)
        {
            // throw new Exception("Test exception");

            // get games from repo
            var gamesFromRepo = await uow.GameRepository.GetGamesAsync(gamesResourceParameters);
               
            // return them
            return Ok(mapper.Map<IEnumerable<GameDto>>(gamesFromRepo));
        }

        // GET: api/Games/5
        [HttpGet("{gameId}")]
        public async Task<ActionResult<Game>> GetGame(int gameId)
        {
          if (uow.GameRepository == null)
          {
              return NotFound();
          }
            var game = await uow.GameRepository.GetAsync(gameId);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{gameId}")]
        //public async Task<IActionResult> PutGame(int gameId, Game game)
        //{
        //    if (gameId != game.GameId)
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
        //        if (!GameExists(gameId))
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

            return CreatedAtAction("GetGame", new { gameId = game.GameId }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{gameId}")]
        public async Task<IActionResult> DeleteGame(int gameId)
        {
            if (uow.GameRepository == null)
            {
                return NotFound();
            }
            var game = await uow.GameRepository.GetAsync(gameId);
            if (game == null)
            {
                return NotFound();
            }

            uow.GameRepository.Remove(game);
            await uow.CompleteAsync();

            return NoContent();
        }

        //private bool GameExists(int gameId)
        //{
        //    return (uow.GameRepository?.Any(e => e.GameId == gameId)).GetValueOrDefault();
        //}
    }
}
