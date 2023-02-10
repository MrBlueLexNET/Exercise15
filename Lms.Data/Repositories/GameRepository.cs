using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Core.ResourceParameters;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    internal class GameRepository : IGameRepository
    {
        private readonly LmsApiContext db;

        public GameRepository(LmsApiContext db)
        {
            this.db = db;
        }
        public void Add(Game gane)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await db.Game
           .ToListAsync();
        }

        public async Task<Game?> GetAsync(int id)
        {
            var query = db.Game.AsQueryable();

            return await query.FirstOrDefaultAsync(c => c.GameId == id);
        }

        //public Task<PagedList<Game>> GetGamesAsync(IGamesResourceParameters gamesResourceParameters)
        public async Task<IEnumerable<Game>> GetGamesAsync(
        IGamesResourceParameters gamesResourceParameters)
        {
      
            if (gamesResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(gamesResourceParameters));
            }

            if (string.IsNullOrWhiteSpace(gamesResourceParameters.Name)
                && string.IsNullOrWhiteSpace(gamesResourceParameters.SearchQuery))
            {
                return await GetAllAsync();
            }

            // collection to start from
            var collection = db.Game as IQueryable<Game>;

            if (!string.IsNullOrWhiteSpace(gamesResourceParameters.Name))
            {
                var gameName = gamesResourceParameters.Name.Trim();
                collection = collection.Where(a => a.Name == gameName);
            }

            if (!string.IsNullOrWhiteSpace(gamesResourceParameters.SearchQuery))
            {
                var searchQuery = gamesResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery)
                   || a.Description.Contains(searchQuery));
            }

            return await collection.ToListAsync();
        }



        public void Remove(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            db.Game.Remove(game);
        }

        public void Update(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
