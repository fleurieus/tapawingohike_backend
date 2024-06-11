using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITeamRepository _teamRepository;

        public AuthService(IAuthRepository authRepository, ITeamRepository teamRepository)
        {
            _authRepository = authRepository;
            _teamRepository = teamRepository;
        }

        public async Task<IActionResult> LoginTeamAsync(string teamCode)
        {
            var team = await _teamRepository.FindTeamByCodeAsync(teamCode);
            
            if (team == null)
                return new NotFoundObjectResult(new { message = "The desired user is not found" });

            return await _authRepository.LoginTeamAsync(teamCode);
        }
    }
}
