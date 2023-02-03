using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string Title { get; set; } = string.Empty;

        //public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        // 1/1/0001 12:00:00 AM.
        //public DateTime EndDate { get; set; } = DateTime();
        //UnixEpoch defines the point in time when Unix time is equal to 0.
        //The value of this constant is equivalent to 00:00:00.0000000 UTC, January 1, 1970, in the Gregorian calendar.
        //public static readonly DateTime UnixEpoch;

        //Navigation properties
        public ICollection<Game>? Games { get; set; } //Declare nullable

    }
}
