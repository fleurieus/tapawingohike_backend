using Microsoft.AspNetCore.Identity;
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

        public async Task<Team> UpdateTeamOnEditionAsync(Team existingTeam, UpdateTeamDto team)
        {
            if (team.Name != null)
            {
                existingTeam.Name = team.Name;
                _context.Teams.Update(existingTeam);
            }

            if (team.Code != null)
            {
                existingTeam.Code = team.Code;
                _context.Teams.Update(existingTeam);
            }

            if (team.ContactEmail != null)
            {
                existingTeam.ContactEmail = team.ContactEmail;
                _context.Teams.Update(existingTeam);
            }

            if (team.ContactName != null)
            {
                existingTeam.ContactName = team.ContactName;
                _context.Teams.Update(existingTeam);
            }

            if (team.ContactPhone != null)
            {
                existingTeam.ContactPhone = team.ContactPhone;
                _context.Teams.Update(existingTeam);
            }

            if (team.Online != null)
            {
                existingTeam.Online = (bool)team.Online;
                _context.Teams.Update(existingTeam);
            }

            await _context.SaveChangesAsync();

            return existingTeam;
        }

        public async Task<bool> DeleteTeamOnEditionAsync(int editionId, int teamId)
        {
            try
            {
                _context.Teams.Remove(await GetTeamOnEditionAsync(editionId, teamId));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
