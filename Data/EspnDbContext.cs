using Microsoft.EntityFrameworkCore;
using EspnBackend.Models;
using EspnBackend.DTO;

namespace EspnBackend.Data
{
   public class EspnDbContext : DbContext
{
    // Your existing DbSets
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<College> Colleges { get; set; }
    public DbSet<NFLGame> NFLGames { get; set; }
    public DbSet<PlayerStats> PlayerStats { get; set; }
    public DbSet<PlayerTeamHistory> PlayerTeamHistory { get; set; }
    public DbSet<CoachTeamHistory> CoachTeamHistory { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Stadium> Stadiums { get; set; }
    public DbSet<TeamTitle> TeamTitles { get; set; }
    public DbSet<Title> Titles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Your existing composite keys
        modelBuilder.Entity<PlayerTeamHistory>()
            .HasKey(p => new { p.PlayerId, p.TeamId });

        modelBuilder.Entity<CoachTeamHistory>()
            .HasKey(c => new { c.CoachId, c.TeamId });

        modelBuilder.Entity<TeamTitle>()
            .HasKey(tt => new { tt.TeamId, tt.TitleId });
    }

    public EspnDbContext(DbContextOptions<EspnDbContext> options) : base(options) { }
}

}