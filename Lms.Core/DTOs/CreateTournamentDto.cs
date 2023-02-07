using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.DTOs
{
    public class CreateTournamentDto
    {
        public string Title { get; set; } = string.Empty;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}
