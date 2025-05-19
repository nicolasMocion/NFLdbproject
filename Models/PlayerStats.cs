namespace EspnBackend.Models{

    public class PlayerStats
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int Yards { get; set; }
        public int Touchdowns { get; set; }
        public int Interceptions { get; set; }
    }
}

 //All Time

