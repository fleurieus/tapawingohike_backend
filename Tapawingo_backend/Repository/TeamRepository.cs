using Microsoft.EntityFrameworkCore;
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

        public ICollection<Team> GetTeamsOnEdition(int editionId)
        {
            return _context.Teams.Where(t => t.EditionId == editionId).ToList();
        }

        public async Task<Team> GetTeamOnEditionAsync(int editionId, int teamId)
        {
            return await _context.Teams.FirstOrDefaultAsync(t => t.EditionId == editionId && t.Id == teamId);
        }

        public bool TeamExists(int teamId)
        {
            bool teamExists = _context.Teams.Any(u => u.Id == teamId);
            return teamExists;
        }

        public async Task<Team> CreateTeamOnEditionAsync(int editionId, CreateTeamDto createTeam)
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
