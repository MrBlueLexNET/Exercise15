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
using Lms.Api.Helpers;
using Lms.Core.ResourceParameters;
using Lms.Api.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Lms.Api.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly UoW uow;
        private readonly IMapper mapper;
        private readonly IPropertyCheckerService propertyCheckerService;
        private readonly ProblemDetailsFactory problemDetailsFactory;

        public GamesController(LmsApiContext context, IMapper mapper, IPropertyCheckerService propertyCheckerService,
        ProblemDetailsFactory problemDetailsFactory)
        {
            uow = new UoW(context);
            this.mapper = mapper;
            this.problemDetailsFactory = problemDetailsFactory;
            this.propertyCheckerService = propertyCheckerService;
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
        //public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(

        //Data Shaping || GamesResourceParameters + Fields (Name, Description) OK
        public async Task<IActionResult> GetGames(
        [FromQuery] GamesResourceParameters gamesResourceParameters)
        {
            // throw new Exception("Test exception");


            if (!propertyCheckerService.TypeHasProperties<GameDto>
             (gamesResourceParameters.Fields))
            {
                return BadRequest(
                    problemDetailsFactory.CreateProblemDetails(HttpContext,
                        statusCode: 400,
                        detail: $"Not all requested data shaping fields exist on " +
                        $"the resource: {gamesResourceParameters.Fields}"));
            }

            // get games from repo
            var gamesFromRepo = await uow.GameRepository.GetGamesAsync(gamesResourceParameters);
               
            // return them
            return Ok(mapper.Map<IEnumerable<GameDto>>(gamesFromRepo)
                .ShapeData(gamesResourceParameters.Fields));
        }

        // GET: api/Games/5
        //Data Shaping Singel Object
        [HttpGet("{gameId}" , Name= "GetGame")]
        public async Task<IActionResult> GetGame(int gameId, string? fields)
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

            // create links
            //var links = CreateLinksForGame(gameId, fields);

            // add 
            //var returnGameAndLinks = game as IDictionary<string, object?>;

            //returnGameAndLinks.Add("links", links);

            // return
            //return Ok(returnGameAndLinks);
            // return them
            return Ok(mapper.Map<GameDto>(game)
                .ShapeData(fields));

            //return game;
        }

        //HATEOAS (Hypermedia as the Engine of Application State) Add links
        private IEnumerable<LinkDto> CreateLinksForGame(int gameId, string? fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new(Url.Link("GetGames", new { gameId }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new(Url.Link("GetGames", new { gameId, fields }),
                  "self",
                  "GET"));
            }

            links.Add(
                  new(Url.Link("CreateTournamentForGame", new { gameId }),
                  "create_tournament_for_game",
                  "POST"));
            links.Add(
                 new(Url.Link("GetTournamentsForGame", new { gameId }),
                 "games",
                 "GET"));

            return links;
        }

        private string? CreateAuthorsResourceUri(
        GamesResourceParameters gamesResourceParameters,
        ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAuthors",
                        new
                        {
                            fields = gamesResourceParameters.Fields,
                            //orderBy = gamesResourceParameters.OrderBy,
                            pageNumber = gamesResourceParameters.PageNumber - 1,
                            pageSize = gamesResourceParameters.PageSize,
                            mainCategory = gamesResourceParameters.Name,
                            searchQuery = gamesResourceParameters.SearchQuery
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors",
                        new
                        {
                            fields = gamesResourceParameters.Fields,
                            //orderBy = gamesResourceParameters.OrderBy,
                            pageNumber = gamesResourceParameters.PageNumber + 1,
                            pageSize = gamesResourceParameters.PageSize,
                            mainCategory = gamesResourceParameters.Name,
                            searchQuery = gamesResourceParameters.SearchQuery
                        });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetAuthors",
                        new
                        {
                            fields = gamesResourceParameters.Fields,
                            //orderBy = gamesResourceParameters.OrderBy,
                            pageNumber = gamesResourceParameters.PageNumber,
                            pageSize = gamesResourceParameters.PageSize,
                            mainCategory = gamesResourceParameters.Name,
                            searchQuery = gamesResourceParameters.SearchQuery
                        });
            }
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
