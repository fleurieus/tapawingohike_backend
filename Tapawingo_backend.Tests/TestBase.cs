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
            var organisation3 = new Organisation { Name = "TestForUpdate", ContactPerson = "testPerson", ContactEmail = "testEmail" };
            var organisation4 = new Organisation { Name = "TestForDelete1", ContactPerson = "testPerson", ContactEmail = "testEmail" };
            var organisation5 = new Organisation { Name = "TestForDelete2", ContactPerson = "testPerson", ContactEmail = "testEmail" };
            context.Organisations.Add(organisation1);
            context.Organisations.Add(organisation2);
            context.Organisations.Add(organisation3);
            context.Organisations.Add(organisation4);
            context.Organisations.Add(organisation5);
            context.SaveChanges();

            // Events
            var event1 = new Event { Name = "TestEvent1", OrganisationId = organisation1.Id };
            var event2 = new Event { Name = "TestEvent2", OrganisationId = organisation1.Id };
            var event3 = new Event { Name = "TestForDelete", OrganisationId = organisation5.Id };
            context.Events.Add(event1);
            context.Events.Add(event2);
            context.Events.Add(event3);
            context.SaveChanges();

            // Editions
            var edition1 = new Edition { Name = "TestEdition1", StartDate = DateTime.Now, EndDate = DateTime.Now, Event = event1 };
            var edition2 = new Edition { Name = "TestEdition2", StartDate = DateTime.Now, EndDate = DateTime.Now, Event = event1 };
            var edition3 = new Edition { Name = "TestEdition3", StartDate = DateTime.Now, EndDate = DateTime.Now, Event = event2 };
            context.Editions.Add(edition1);
            context.Editions.Add(edition2);
            context.Editions.Add(edition3);
            context.SaveChanges();

            // Teams
            var team1 = new Team { Name = "TestTeam1", Code = "test", ContactEmail = "testEmail", ContactName = "testName", ContactPhone = "0123456789", Online = false, EditionId = edition1.Id };
            var team2 = new Team { Name = "TestTeam2", Code = "test", ContactEmail = "testEmail", ContactName = "testName", ContactPhone = "0123456789", Online = false, EditionId = edition1.Id };
            context.Teams.Add(team1);
            context.Teams.Add(team2);
            context.SaveChanges();

            // Locationlogs
            var locationlog1 = new Locationlog { Latitude = 1, Longitude = 1, TeamId = team1.Id, Timestamp = DateTime.Now };
            var locationlog2 = new Locationlog { Latitude = 2, Longitude = 2, TeamId = team1.Id, Timestamp = DateTime.Now };
            context.Locationlogs.Add(locationlog1);
            context.Locationlogs.Add(locationlog2);
            context.SaveChanges();

            // Routes
            var route1 = new TWRoute { Name = "TestRoute1", EditionId = edition1.Id };
            var route2 = new TWRoute { Name = "TestRoute2", EditionId = edition2.Id };
            var route3 = new TWRoute { Name = "TestRoute3", EditionId = edition2.Id };
            var route4 = new TWRoute { Name = "TestForDelete", EditionId = edition1.Id };
            var route5 = new TWRoute { Name = "TestForDelete2", EditionId = edition1.Id };
            context.Routes.Add(route1);
            context.Routes.Add(route2);
            context.Routes.Add(route3);
            context.Routes.Add(route4);
            context.Routes.Add(route5);
            context.SaveChanges();
            
            //routeparts
            var routepart1 = new Routepart { Name = "Routepart1", Final = false, Order = 1, RoutepartFullscreen = false, RoutepartZoom = false, RouteType = "normal", RouteId = route3.Id };
            var routepart2 = new Routepart { Name = "Routepart2", Final = false, Order = 1, RoutepartFullscreen = false, RoutepartZoom = false, RouteType = "normal", RouteId = route3.Id };
            context.Routeparts.Add(routepart1);
            context.Routeparts.Add(routepart2);
            context.SaveChanges();

            //////routes
            //////teamrouteparts

            //////userevents
            //////userorganisations

            // Save data to the database
            context.SaveChanges();

            // Test users on organisation
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


            // Test users on event
            // Users
            var user3 = new User { FirstName = "Testuser3", LastName = "Usertest3", UserName = "test3@gmail.com", Email = "test3@gmail.com", SecurityStamp = Guid.NewGuid().ToString() };
            context.Users.Add(user3);
            context.SaveChanges();

            // Add user to event
            context.UserEvents.Add(new UserEvent { Event = event1, User = user3 });
            context.SaveChanges();

            var user4 = new User { FirstName = "Testuser4", LastName = "Usertest4", UserName = "test4@gmail.com", Email = "test4@gmail.com", SecurityStamp = Guid.NewGuid().ToString() };
            context.Users.Add(user4);
            context.SaveChanges();

            // Add user to event
            context.UserEvents.Add(new UserEvent { Event = event1, User = user4 });
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
