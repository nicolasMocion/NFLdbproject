namespace EspnBackend.Models
{
    public class College {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }

        public ICollection<Player> Players { get; set; }
    }

}