using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.DTOs
{
    public class CreateGameDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public ICollection<CreateTournamentDto> Courses { get; set; } = new List<CreateTournamentDto>();
    }
}
