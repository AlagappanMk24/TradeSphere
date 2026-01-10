using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TradeSphere.Application.Contracts.Interfaces.Repositories;
using TradeSphere.Application.Contracts.Interfaces.Services;
using TradeSphere.Application.Settings;

namespace TradeSphere.Infrastructure.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IOptions<JwtSettings> jwtOptions, IRefreshTokenRepository refreshTokenRepository) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JwtSettings _jwtSettings  = jwtOptions.Value;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.MobilePhone, user.PhoneNumber!)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // Use the Key from JwtSettings
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                claims: claims,
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public RefreshToken GenerateRefreshToken(int userId, bool rememberMe)
        {
            var days = rememberMe ? _jwtSettings.RefreshTokenExpirationDays * 2 : _jwtSettings.RefreshTokenExpirationDays;
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpireOn = DateTime.UtcNow.AddDays(days),
                CreatedOn = DateTime.UtcNow,
                ApplicationUserId = userId,
            };
        }
        public async Task<(string AccessToken, RefreshToken RefreshToken)> RefreshTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token) ?? throw new Exception("Invalid refresh token");
            if (refreshToken.RevokedOn.HasValue)
                throw new Exception("Token has been revoked");

            if (refreshToken.ExpireOn < DateTime.UtcNow)
                throw new Exception("Token has expired");

            await _refreshTokenRepository.RevokeAsync(refreshToken);

            var user = refreshToken.ApplicationUser;
            var newAccessToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user.Id, refreshToken.RememberMe);
            await _refreshTokenRepository.AddAsync(newRefreshToken);
            return (newAccessToken, newRefreshToken);
        }
    }
}