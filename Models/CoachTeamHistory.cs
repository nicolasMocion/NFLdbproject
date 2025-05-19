using Microsoft.EntityFrameworkCore;

namespace EspnBackend.Models{

    public class CoachTeamHistory {
        public Coach Coach { get; set; }
        public Team Team { get; set; }
        public int CoachId { get; set; }
        public int TeamId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

}   