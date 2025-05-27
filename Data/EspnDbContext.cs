using Microsoft.EntityFrameworkCore;
using EspnBackend.Models;
using EspnBackend.DTO;
using EspnBackend.Security;

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
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite keys
            modelBuilder.Entity<PlayerTeamHistory>()
                .HasKey(p => new { p.PlayerId, p.TeamId, p.StartDate });

            modelBuilder.Entity<CoachTeamHistory>()
                .HasKey(c => new { c.CoachId, c.TeamId, c.StartDate });

            modelBuilder.Entity<TeamTitle>()
                .HasKey(tt => new { tt.TeamId, tt.TitleId, tt.YearWon });

            // Exclude pre-existing tables from migrations
            modelBuilder.Entity<User>().ToTable("Users", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<UserLoginHistory>().ToTable("UserLoginHistory", t => t.ExcludeFromMigrations());
        }

        public EspnDbContext(DbContextOptions<EspnDbContext> options) : base(options) { }
    }
}