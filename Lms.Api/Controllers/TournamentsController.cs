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

namespace Lms.Api.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        // private readonly LmsApiContext _context;
        private readonly UoW uow;
        private readonly IMapper mapper;

        public TournamentsController(LmsApiContext context, IMapper mapper)
        {
            //_context = context;
            uow = new UoW(context);
            this.mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Tournament>>> GetTournament()
        //{
        //    var tournaments = await uow.TournamentRepository.GetAllAsync();
        //    return Ok(tournaments);
        //}
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament()
        {
            var tournaments = await uow.TournamentRepository.GetAllAsync();
            
            var dto = mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(dto);

        }


        // GET: api/Tournaments/5
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Tournament>> GetTournament(int id)
        {
          if (uow.TournamentRepository == null)
          {
              return NotFound();
          }
            var tournament = await uow.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return tournament;
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //public async Task<IActionResult> PutTournament(int id, Tournament tournament)
        //{
        //    if (id != tournament.TournamentId)
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
        //        if (!TournamentExists(id))
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
        public async Task<ActionResult<CreateTournamentDto>> CreateTournament(CreateTournamentDto dto)
        {
           

            var tournament = mapper.Map<Tournament>(dto);
            uow.TournamentRepository.Add(tournament);
            await uow.CompleteAsync();

            return CreatedAtAction(nameof(GetTournament), new { id = tournament.TournamentId }, dto);
        }

        //  if (uow.TournamentRepository == null)
        //  {
        //      return Problem("Entity set 'LmsApiContext.Tournament'  is null.");
        //  }
        //    uow.TournamentRepository.Add(tournament);
        //    await uow.CompleteAsync();

        //    return CreatedAtAction("GetTournament", new { id = tournament.TournamentId }, tournament);
        //}

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            if (uow.TournamentRepository == null)
            {
                return NotFound();
            }
            var tournament = await uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            uow.TournamentRepository.Remove(tournament);
            await uow.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TournamentDto>> PatchEvent(int id, JsonPatchDocument<TournamentDto> patchDocument)
        {
            var tournament = await uow.TournamentRepository.GetAsync(id);
            if (tournament == null) return NotFound();

            var dto = mapper.Map<TournamentDto>(tournament);

            patchDocument.ApplyTo(dto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            mapper.Map(dto, tournament);
            await uow.CompleteAsync();

            return Ok(mapper.Map<TournamentDto>(tournament));
        }

        //private bool TournamentExists(int id)
        //{
        //    return (uow.TournamentRepository?.Any(e => e.TournamentId == id)).GetValueOrDefault();
        //}
    }
}
