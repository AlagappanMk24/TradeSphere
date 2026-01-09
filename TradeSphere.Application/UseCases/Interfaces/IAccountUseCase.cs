namespace TradeSphere.Application.UseCases.Interfaces
{
    public interface IAccountUseCase
    {
        Task<UserProfileDto> GetProfile(string UserId);
        Task<UserProfileDto> UpdateProfile(string UserId, UpdateProfileDto updateProfile);
        Task<string> ChangePassword(string email, string currentPassword, string newPassword);
        Task<string> RequestChangeEmail(string currentEmail, string newEmail);
    }
}