// AdminGameDtos.cs
namespace EspnBackend.DTO
{
    // For listing games in admin view
    public class AdminGameDto
    {
        public int Id { get; set; }
        public DateTime GameDate { get; set; }
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public int SeasonId { get; set; }
        public int SeasonYear { get; set; }
    }

    // For creating/updating games
    public class AdminGameEditDto
    {
        public DateTime GameDate { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public int SeasonId { get; set; }
    }

    // For dropdowns
    public class AdminTeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AdminSeasonDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
    }
}