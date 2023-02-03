using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
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

        public Task<IEnumerable<Game>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Game> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Game game)
        {
            throw new NotImplementedException();
        }

        public void Update(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
