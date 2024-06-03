using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Tests
{
    public class TestBase : IDisposable
    {
        protected DataContext CreateDbContext()
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

            //Create test data
            //////destinations
            //////editions
            //////events
            //////files
            //////locationlogs
            //////organisations
            var organisation1 = new Organisation { Name = "TestOrganisation1" };
            //////routeparts
            //////routes
            //////teamrouteparts
            //////teams
            //////userevents
            //////userorganisations

            // Insert test data
            //////destinations
            //////events
            //////files
            //////locationlogs
            //////organisations
            context.Organisations.Add(organisation1);
            //////routeparts
            //////routes
            //////teamrouteparts
            //////teams
            //////userevents
            //////userorganisations


            //Save Data
            context.SaveChanges();
        }

        public void Dispose()
        {
            using (var context = CreateDbContext())
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
