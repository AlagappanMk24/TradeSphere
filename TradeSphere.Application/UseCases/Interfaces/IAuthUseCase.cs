namespace TradeSphere.Application.UseCases.Interfaces
{
    public interface IAuthUseCase
    {
        Task<UserResultDto> RegisterUser(UserRegisterDto registerUser);
        Task<UserResultDto> LoginUser(UserLoginDto loginUser);
        Task<string> ConfirmEmail(string userId, string token);
        Task<string> ForgetPassword(string email);
        Task<string> ResetPassword(ResetPasswordDto resetPassword);
        Task<string> ConfirmEmailForAfterChanging(string userId, ConfirmChangeEmailRequest emailChange);
        Task<(string Token, RefreshToken RefreshToken)> RefreshToken(RefreshTokenRequest request);
        Task<string> LogoutAsync(string userId);
    }
}
