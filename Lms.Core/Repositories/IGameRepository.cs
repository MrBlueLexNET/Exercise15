using Lms.Core.Entities;
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
        void Add(Game gane);
        void Update(Game game);
        void Remove(Game game);
        //Test Pagination
        Task<PagedList<Game>> GetGamesAsync(GamesResourceParameters gamesResourceParameters);
    }
}
