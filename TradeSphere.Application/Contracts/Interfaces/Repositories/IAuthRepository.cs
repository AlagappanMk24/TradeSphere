namespace TradeSphere.Application.Contracts.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        public Task AddDefaultRole(ApplicationUser user, string roleName);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByUserIdAsync(string userId);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        public Task<bool> CheckIfUserNameExistAsync(string userName);
        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);
        Task<bool> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        Task ForgetPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPassword);
        Task<string> ChangeEmailAsync(string userId, ConfirmChangeEmailRequest emailChange);
        Task LogoutAsync(string userId);
    }
}