using Bogus.DataSets;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly LmsApiContext db;

        public TournamentRepository(LmsApiContext db)
        {
            this.db = db;
        }
        public void Add(Tournament tournament)
        {
            if (tournament is null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

             db.AddAsync(tournament);
        }

        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tournament> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tournament>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return await db.Tournament
            .ToListAsync();
        }

        public async Task<Tournament> GetAsync(int id)
        {


            //var tournament = await db.Tournament.FindAsync(id);

            //if (tournament == null)
            //{
            //    throw new ArgumentException($"'{nameof(id)}' cannot be found", nameof(id));
            //}
          
            var query = db.Tournament
                    .Include(c => c.Games)
                    .AsQueryable();

            return await query.FirstOrDefaultAsync(c => c.TournamentId == id);
        }

        public void Remove(Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            db.Tournament.Remove(tournament);
        }

        public void Update(Tournament tournament)
        {
            throw new NotImplementedException();
        }

        private bool TournamentExists(int id)
        {
            return (db.Tournament?.Any(e => e.TournamentId == id)).GetValueOrDefault();
        }

        //public async Task<IEnumerable<Tournament>> GetTournamentsAsync(IEnumerable<Guid> tournamentIds)
        //{
        //    if (tournamentIds == null)
        //    {
        //        throw new ArgumentNullException(nameof(tournamentIds));
        //    }

        //    return await db.Tournament.Where(t => tournamentIds.Contains(t.TournamentId))
        //                              .OrderBy(t => t.Title)
        //                              .ToListAsync();
        //}

       
       
    }
}
