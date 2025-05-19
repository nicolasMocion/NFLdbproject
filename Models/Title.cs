namespace EspnBackend.Models{
    public class Title{
        public int Id { get; set; }
        public string Name { get; set; } // e.g. "Super Bowl", "AFC Championship"
        public string Description { get; set; } // Optional: extra details
        public ICollection<TeamTitle> TeamTitles { get; set; }
    }
}