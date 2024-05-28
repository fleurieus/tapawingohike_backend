using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Data
{
    public class DataContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<UserOrganisation> UserOrganisations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<TWRoute> Routes { get; set; }
        public DbSet<Locationlog> Locationlogs { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Routepart> Routeparts { get; set; }
        public DbSet<TeamRoutepart> TeamRouteparts { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<TWFile> Files { get; set; }

        // On Model Creating needs to be added when creating a many-to-many relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserOrganisation>()
                .HasKey(bc => new { bc.UserId, bc.OrganisationId });
            modelBuilder.Entity<UserOrganisation>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.Organisations)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserOrganisation>()
                .HasOne(bc => bc.Organisation)
                .WithMany(b => b.Users)
                .HasForeignKey(bc => bc.OrganisationId);


            modelBuilder.Entity<UserEvent>()
                .HasKey(bc => new { bc.UserId, bc.EventId });
            modelBuilder.Entity<UserEvent>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.Events)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserEvent>()
                .HasOne(bc => bc.Event)
                .WithMany(b => b.Users)
                .HasForeignKey(bc => bc.EventId);


            modelBuilder.Entity<TeamRoutepart>()
                .HasKey(bc => new { bc.TeamId, bc.RoutepartId });
            modelBuilder.Entity<TeamRoutepart>()
                .HasOne(bc => bc.Team)
                .WithMany(b => b.TeamRouteparts)
                .HasForeignKey(bc => bc.TeamId);
            modelBuilder.Entity<TeamRoutepart>()
                .HasOne(bc => bc.Routepart)
                .WithMany(b => b.TeamRouteparts)
                .HasForeignKey(bc => bc.RoutepartId);
        }
    }
}
