using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lms.Core.DTOs
{
    public class TournamentDto
    {
        private DateTime _startDate;
        
        
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        public DateTime EndDate  => StartDate.AddMonths(3);
       
    }


}
