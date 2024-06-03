using Tapawingo_backend.Data; // Importing the Data namespace
using Tapawingo_backend.Models; // Importing the Models namespace

namespace Tapawingo_backend.Repository
{
    // Repository class for interacting with team data
    public class TeamRepository : ITeamRepository
    {
        private readonly DataContext _context; // Database context for accessing data

        // Constructor to initialize the repository with the database context
        public TeamRepository(DataContext context)
        {
            _context = context;
        }

        // Method to add a new team to the repository
        public async Task<Team> AddTeam(Team team)
        {
            // Add the team object to the Teams DbSet in the database context
            _context.Teams.Add(team);
            
            // Save the changes to the database asynchronously
            await _context.SaveChangesAsync();
            
            // Return the added team object
            return team;
        }
    }
}