using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
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

        public async Task<Team> CreateTeamOnEdition(int editionId, CreateTeamDto createTeam)
        {
            Team team = new Team
            {
                Code = createTeam.Code,
                Name = createTeam.Name,
                ContactEmail = createTeam.ContactEmail,
                ContactName = createTeam.ContactName,
                ContactPhone = createTeam.ContactPhone,
                Online = createTeam.Online,
                EditionId = editionId
            };
            
            _context.Teams.Add(team);
            _context.SaveChanges();
            
            return team;
        }
    }
}
