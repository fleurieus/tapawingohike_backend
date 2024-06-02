using System.Threading.Tasks;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<Team> CreateTeam(CreateTeamDto model)
        {
            var team = new Team
            {
                Name = model.Name,
                Code = model.Code,
                ContactName = model.ContactName,
                ContactEmail = model.ContactEmail,
                ContactPhone = model.ContactPhone,
                Online = model.Online
            };

            return await _teamRepository.AddTeam(team);
        }
    }
}
