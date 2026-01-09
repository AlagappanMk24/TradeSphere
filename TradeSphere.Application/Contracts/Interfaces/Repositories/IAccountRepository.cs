namespace TradeSphere.Application.Contracts.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<ApplicationUser> GetProfileAsync(string userId);
        Task<ApplicationUser> UpdateProfileAsync(ApplicationUser ApplicationUser);
        Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task RequestChangeEmailAsync(string userId, string newEmail);
    }
}
