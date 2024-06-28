using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new BadHttpRequestException("Invalid credentials");

            JwtSecurityToken token = await GenerateJwt(user.Id);

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
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);

            if (principal?.Identity is null)
                throw new UnauthorizedAccessException();

            var userEmail = principal.Claims.Single(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException();

            var token = await GenerateJwt(user.Id);

            return new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = model.RefreshToken
            };
        }

        public async Task<CustomResponse> Revoke(HttpContext httpContext)
        {
            var username = httpContext.User.Claims.ElementAt(3).Value;

            if (username is null)
                throw new UnauthorizedAccessException();

            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                throw new UnauthorizedAccessException();

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return new CustomResponse("Revoke succesfull");
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

        private async Task<JwtSecurityToken> GenerateJwt(string userId)
        {
            var claims = await GetAllValidClaims(userId);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Secret not configured")));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(2),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private async Task<List<Claim>> GetAllValidClaims(string userId)
        {
            var _options = new IdentityOptions();

            var user = await _userManager.FindByIdAsync(userId);

            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("firstname", user.FirstName),
                new Claim("lastname", user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Get claims that are assigned to user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            return claims;
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
