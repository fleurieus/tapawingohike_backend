using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public interface ITeamRepository
    {
        Team CreateTeamOnEdition(int editionId, CreateTeamDto team);
        public async Task<Team?> FindTeamByCodeAsync(string teamCode);
    }
}
