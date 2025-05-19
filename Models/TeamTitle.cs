using Microsoft.EntityFrameworkCore;

namespace EspnBackend.Models{

    public class TeamTitle {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int TitleId { get; set; }
        public Title Title { get; set; }

        public int YearWon { get; set; } // e.g., 2023
    }

}
