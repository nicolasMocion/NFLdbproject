using Microsoft.EntityFrameworkCore;

namespace EspnBackend.Models{

    public class PlayerTeamHistory {
        public Player Player { get; set; }
        public int PlayerId { get; set; }
        public Team Team { get; set; }
        public int TeamId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

}