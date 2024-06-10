using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Models;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Tapawingo_backend.Tests
{
    [CollectionDefinition("Database collection")]
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            // Create the database once before running all tests
            Context = CreateDbContext();
        }

        public DataContext Context { get; private set; }

        private DataContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseMySQL("server=localhost;database=TapawingoDB_Test;user=root;password=mysql");

            var context = new DataContext(options.Options);
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            SeedData(context);
            return context;
        }

        private void SeedData(DataContext context)
        {
            // Clear existing data
            context.Users.RemoveRange(context.Users);
            context.Organisations.RemoveRange(context.Organisations);
            context.UserOrganisations.RemoveRange(context.UserOrganisations);
            context.Events.RemoveRange(context.Events);
            context.UserEvents.RemoveRange(context.UserEvents);
            context.Routes.RemoveRange(context.Routes);
            context.Editions.RemoveRange(context.Editions);
            context.Locationlogs.RemoveRange(context.Locationlogs);
            context.Teams.RemoveRange(context.Teams);
            context.Routeparts.RemoveRange(context.Routeparts);
            context.TeamRouteparts.RemoveRange(context.TeamRouteparts);
            context.Destinations.RemoveRange(context.Destinations);
            context.Files.RemoveRange(context.Files);
            context.SaveChanges();

            // Create test data
            // Organisations
            var organisation1 = new Organisation { Name = "TestOrganisation1", ContactPerson = "testPerson", ContactEmail = "testEmail" };
            var organisation2 = new Organisation { Name = "TestOrganisation2", ContactPerson = "testPerson", ContactEmail = "testEmail" };
            context.Organisations.Add(organisation1);
            context.Organisations.Add(organisation2);
            context.SaveChanges();

            // Events
            var event1 = new Event { Name = "TestEvent1", OrganisationId = organisation1.Id };
            context.Events.Add(event1);
            context.SaveChanges();

            // Editions
            var edition1 = new Edition { Name = "TestEdition1", StartDate = DateTime.Now, EndDate = DateTime.Now };
            var edition2 = new Edition { Name = "TestEdition2", StartDate = DateTime.Now, EndDate = DateTime.Now };
            context.Editions.Add(edition1);
            context.Editions.Add(edition2);
            context.SaveChanges();

            // Routes
            var route1 = new TWRoute { Name = "TestRoute1", EditionId = edition1.Id };
            var route2 = new TWRoute { Name = "TestRoute2", EditionId = edition2.Id };
            context.Routes.Add(route1);
            context.Routes.Add(route2);
            context.SaveChanges();
            //////routeparts
            //////routes
            //////teamrouteparts
            //////teams

            //////userevents
            //////userorganisations

            // Save data to the database
            context.SaveChanges();

            // Users
            var user1 = new User { FirstName = "Testuser1", LastName = "Usertest1", UserName = "test1@gmail.com", Email = "test1@gmail.com", SecurityStamp = Guid.NewGuid().ToString() };
            context.Users.Add(user1);
            context.SaveChanges();

            // Add user to organisation
            context.UserOrganisations.Add(new UserOrganisation { Organisation = organisation1, User = user1 });
            context.SaveChanges();

            var user2 = new User { FirstName = "Testuser2", LastName = "Usertest2", UserName = "test2@gmail.com", Email = "test2@gmail.com", SecurityStamp = Guid.NewGuid().ToString() };
            context.Users.Add(user2);
            context.SaveChanges();

            // Add user to organisation
            context.UserOrganisations.Add(new UserOrganisation { Organisation = organisation1, User = user2 });
            context.SaveChanges();


            //Save Data
            context.SaveChanges();
        }

        public void Dispose()
        {
            // Dispose of the database after all tests have finished
            Context.Dispose();
        }
    }

    public class TestBase : IClassFixture<DatabaseFixture>
    {
        protected DataContext Context => Fixture.Context;

        protected TestBase(DatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        protected DatabaseFixture Fixture { get; }
    }
}
