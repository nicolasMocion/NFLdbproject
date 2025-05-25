namespace EspnBackend.Models{

    public class Player 
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Position { get; set; } = null!;
        public College College { get; set; }
        public int CollegeId { get; set; }
        public Team Team { get; set; } = null!;
        public int TeamId { get; set; }
        public int HomeTownCityId { get; set; }
        public City HomeTownCity { get; set; }
        public ICollection<PlayerTeamHistory> TeamHistory { get; set; }
    }
}

