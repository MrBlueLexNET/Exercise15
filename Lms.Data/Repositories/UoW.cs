using Lms.Core.Repositories;
using Lms.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class UoW : IUoW
    {
        private readonly LmsApiContext db;
        
        public ITournamentRepository TournamentRepository { get; }

        public IGameRepository GameRepository { get; }
        public UoW(LmsApiContext db)
        {
            this.db = db;
            TournamentRepository = new TournamentRepository(db);
            GameRepository = new GameRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
