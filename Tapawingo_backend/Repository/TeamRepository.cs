using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{ 
    public class TeamRepository : ITeamRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public TeamRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Team CreateTeamOnEdition(int editionId, CreateTeamDto createTeam)
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

        public async Task<Team?> FindTeamByCodeAsync(string teamCode)
        {
            var team = await _context.Teams.FindAsync(teamCode);
            return team;
        }
    }
}
