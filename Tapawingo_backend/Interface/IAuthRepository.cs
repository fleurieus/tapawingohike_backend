using Tapawingo_backend.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Tapawingo_backend.Interface
{
    public interface IAuthRepository
    {
        Task<LoginResponse> Login([FromBody] LoginDto model);
        Task<LoginResponse> Refresh([FromBody] RefreshDto model);
        Task<CustomResponse> Revoke(HttpContext httpContext);
    }
}
