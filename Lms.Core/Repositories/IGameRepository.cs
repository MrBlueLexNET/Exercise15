using Lms.Core.Entities;
using Lms.Core.ResourceParameters;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(Game game);
        void Update(Game game);
        void Remove(Game game);
        //Test Pagination
        //Task<PagedList<Game>> GetGamesAsync(IGamesResourceParameters gamesResourceParameters);
        //Test Filter by Name and Search Name and Description
        Task<IEnumerable<Game>> GetGamesAsync(IGamesResourceParameters gamesResourceParameters);
    }
}
