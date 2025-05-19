namespace EspnBackend.Models
{
    public class NFLGame
    {
        public int Id { get; set; }
        public DateTime GameDate { get; set; }
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; } 
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public Season Season { get; set; }
        public int SeasonId { get; set; }
        
    }
}
