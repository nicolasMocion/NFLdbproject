using System.Collections.Generic;
namespace EspnBackend.Models
{
    public class Team {
        public int Id { get; set; }
        public string Name { get; set; }

        public int StadiumId { get; set; }
        public Stadium Stadium { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public ICollection<PlayerTeamHistory> PlayerHistory { get; set; }
        public ICollection<CoachTeamHistory> CoachHistories { get; set; }
        public ICollection<TeamTitle> Titles { get; set; }
    }
}

