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
using AutoMapper;
using Lms.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Hellang.Middleware.ProblemDetails;

namespace Lms.Api.Controllers
{
    [Route("api/games/{gameId}/tournaments")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
       
        private readonly UoW uow;
        private readonly IMapper mapper;

        public TournamentsController(LmsApiContext context, IMapper mapper)
        {
            uow = new UoW(context);
            this.mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament()
        {
            var tournaments = await uow.TournamentRepository.GetAllAsync();
            
            var dto = mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(dto);

        }

        // GET: api/Tournaments/5
        [HttpGet("{tournamentId}")]
       
        public async Task<ActionResult<Tournament>> GetTournament(int tournamentId)
        {
          if (uow.TournamentRepository == null)
          {
              return NotFound();
          }
            var tournament = await uow.TournamentRepository.GetAsync(tournamentId);

            if (tournament == null)
            {
                return NotFound();
            }

            return tournament;
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{tournamentId}")]
        //public async Task<IActionResult> PutTournament(int tournamentId, Tournament tournament)
        //{
        //    if (tournamentId != tournament.TournamentId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tournament).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TournamentExists(tournamentId))
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

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateTournamentDto>> CreateTournament(int gameId, CreateTournamentDto dto)
        {
            var game = await uow.GameRepository.GetAsync(gameId);

            if (game is null)
            {
                return NotFound(ProblemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Game not exists",
                                                                          detail: $"The game {gameId} doesn't exist"));
            }

            var tournament = mapper.Map<Tournament>(dto);
            uow.TournamentRepository.Add(tournament);
            await uow.CompleteAsync();

            return CreatedAtAction(nameof(GetTournament), new { gameId = game.GameId, tournamentId = tournament.TournamentId }, dto);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{tournamentId}")]
        public async Task<IActionResult> DeleteTournament(int tournamentId)
        {
            if (uow.TournamentRepository == null)
            {
                return NotFound();
            }
            var tournament = await uow.TournamentRepository.GetAsync(tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            uow.TournamentRepository.Remove(tournament);
            await uow.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchEvent(int tournamentId, JsonPatchDocument<TournamentDto> patchDocument)
        {
            var tournament = await uow.TournamentRepository.GetAsync(tournamentId);
            if (tournament == null) return NotFound();

            var dto = mapper.Map<TournamentDto>(tournament);

            patchDocument.ApplyTo(dto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            mapper.Map(dto, tournament);
            await uow.CompleteAsync();

            return Ok(mapper.Map<TournamentDto>(tournament));
        }

        [HttpOptions()]
        public IActionResult GetTournamentsOptions()
        {
            Response.Headers.Add("Allow", "GET,HEAD,POST,PATCH,DELETE,OPTIONS");
            return Ok();
        }

        //private bool TournamentExists(int tournamentId)
        //{
        //    return (uow.TournamentRepository?.Any(e => e.TournamentId == tournamentId)).GetValueOrDefault();
        //}
    }
}
