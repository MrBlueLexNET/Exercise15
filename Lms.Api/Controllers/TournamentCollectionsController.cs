using AutoMapper;
using Lms.Api.Helpers;
using Lms.Core.DTOs;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Lms.Api.Controllers
{
    [Route("api/tournamentcollections")]
    [ApiController]
    public class TournamentCollectionsController : ControllerBase
    {

        private readonly UoW uow;
        private readonly IMapper mapper;

        public TournamentCollectionsController(LmsApiContext context, IMapper mapper) 
        {
            uow = new UoW(context);
            this.mapper = mapper;

        }

        // 1,2,3
        // key1=value1,key2=value2

        //[HttpGet("({tournamentIds})", Name = "GetTournamentCollection")]
        //public async Task<ActionResult<IEnumerable<CreateTournamentDto>>>
        //    GetAuthorCollection(
        //            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        //            [FromRoute] IEnumerable<Guid> tournamentIds)
        //{
        //    var tournamentEntities = await uow.TournamentRepository
        //        .GetTournamentsAsync(tournamentIds);

        //    // do we have all requested authors?
        //    if (tournamentIds.Count() != tournamentEntities.Count())
        //    {
        //        return NotFound();
        //    }

        //    // map
        //    var tournamentsToReturn = mapper.Map<IEnumerable<Tournament>>(tournamentEntities);
        //    return Ok(tournamentsToReturn);
        //}


        [HttpPost]
        public async Task<ActionResult<IEnumerable<Tournament>>> CreateTournamentCollection(
            IEnumerable<CreateTournamentDto> tournamentCollection)
        {
            var tournamentEntities = mapper.Map<IEnumerable<Tournament>>(tournamentCollection);
            foreach (var tournament in tournamentEntities)
            {
                uow.TournamentRepository.Add(tournament);
            }
            await uow.CompleteAsync(); 

            var tournamentCollectionToReturn = mapper.Map<IEnumerable<Tournament>>(
                tournamentEntities);
            var tournamentIdsAsString = string.Join(",",
                tournamentCollectionToReturn.Select(a => a.TournamentId));

            return CreatedAtRoute("GetTournamentCollection",
              new { tournamentIds = tournamentIdsAsString },
              tournamentCollectionToReturn);
        }

    }
}
