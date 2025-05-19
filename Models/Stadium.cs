namespace EspnBackend.Models
{
        public class Stadium {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int CityId { get; set; }
        public City CityName { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}
