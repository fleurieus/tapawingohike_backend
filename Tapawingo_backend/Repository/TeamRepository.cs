using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DataContext _context;

        public TeamRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Team> AddTeam(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }
    }
}
