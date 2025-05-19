namespace EspnBackend.Models 
{
    public class Coach 
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Speciality { get; set; } = null!;

        public Team Team { get; set; }
        public int TeamId { get; set; }
        public ICollection<CoachTeamHistory> TeamHistory { get; set; }
    }

}


