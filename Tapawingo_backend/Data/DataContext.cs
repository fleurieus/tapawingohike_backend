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
        }
    }
}
