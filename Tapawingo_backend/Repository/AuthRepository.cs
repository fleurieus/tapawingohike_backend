using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tapawingo_backend;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new BadHttpRequestException("Invalid credentials");

            var getUserRole = await _userManager.GetRolesAsync(user);

            JwtSecurityToken token = GenerateJwt(model.Email, user.Id, getUserRole.First());

            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);

            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponse> Refresh([FromBody] RefreshDto model)
        {
            Console.WriteLine("Refresh called");

            var principal = GetPrincipalFromExpiredToken(model.AccessToken);

            if (principal?.Identity?.Name is null)
                throw new UnauthorizedAccessException();

            var user = await _userManager.FindByEmailAsync(principal.Identity.Name);

            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException();

            var getUserRole = await _userManager.GetRolesAsync(user);

            var token = GenerateJwt(principal.Identity.Name, user.Id, getUserRole.First());

            Console.WriteLine("Refresh succeeded");

            return new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = model.RefreshToken
            };
        }

        public async Task<CustomResponse> Revoke(HttpContext httpContext)
        {
            var username = httpContext.User.Identity?.Name;

            if (username is null)
                throw new UnauthorizedAccessException();

            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                throw new UnauthorizedAccessException();

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return new CustomResponse("Revoke succesfull");
        }


        public async Task<IActionResult> LoginTeamAsync(string teamCode)
        {
            JwtSecurityToken token = GenerateTeamJwt(teamCode);

            return new ObjectResult(new { message = token.ValidTo, token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        private JwtSecurityToken GenerateTeamJwt(string teamcode)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, teamcode)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Secret not configured")));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(24), // Valid for one day
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var secret = _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Secret not configured");

            var validation = new TokenValidationParameters
            {
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }

        private JwtSecurityToken GenerateJwt(string username, string userId, string role)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, userId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Secret not configured")));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var generator = RandomNumberGenerator.Create();

            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
