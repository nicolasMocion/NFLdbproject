namespace EspnBackend.Models{

    public class Season {
        public int Id { get; set; }
        public int Year { get; set; }

        public ICollection<NFLGame> Games { get; set; }
    }
}