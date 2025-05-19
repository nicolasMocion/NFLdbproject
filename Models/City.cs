namespace EspnBackend.Models
{
    public class City {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Population { get; set; }

        public ICollection<Team> Teams { get; set; }

    }
}