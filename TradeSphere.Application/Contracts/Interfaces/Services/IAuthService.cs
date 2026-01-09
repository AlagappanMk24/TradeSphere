namespace TradeSphere.Application.Contracts.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<string> GenerateJwtToken(ApplicationUser user);
        RefreshToken GenerateRefreshToken(int userId, bool rememberMe);
        Task<(string AccessToken, RefreshToken RefreshToken)> RefreshTokenAsync(string refreshToken);
    }
}