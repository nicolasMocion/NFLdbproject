namespace EspnBackend.DTO{

    public class NFLGameDto
    {
        public int Id { get; set; }
        public string GameDate{ get; set; }
        public string HomeTeamName{ get; set; }
        public string AwayTeamName{ get; set; } 
        public int Year{ get; set; }
        public int HomeScore{ get; set; }
        public int AwayScore{ get; set; }   

    }

}

