namespace TradeSphere.Application.UseCases
{
    public class AuthUseCase(IAuthService authService, IAuthRepository authRepository, IRefreshTokenRepository refreshTokenRepository) : IAuthUseCase
    {
        private readonly IAuthService _authService = authService;
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        public async Task<UserResultDto> RegisterUser(UserRegisterDto registerUser)
        {
            var existingUser = await _authRepository.FindByEmailAsync(registerUser.Email);
            if (existingUser != null)
            {
                throw new Exception("This Email Is Already Exist");
            }

            if (await _authRepository.CheckIfUserNameExistAsync(registerUser.UserName))
            {
                throw new Exception("This UserName Is Already Exist");
            }
            var addUser = new ApplicationUser()
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                PhoneNumber = registerUser.PhoneNumber,
            };
            await _authRepository.CreateAsync(addUser, registerUser.Password);
            await _authRepository.AddDefaultRole(addUser, "User");
            var result = new UserResultDto()
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                Message = "Verify Your Email"
            };
            return result;
        }
        public async Task<UserResultDto> LoginUser(UserLoginDto loginUser)
        {
            var findUser = await _authRepository.FindByEmailAsync(loginUser.Email) ?? throw new Exception("InValid Email Or Password");
            var checkPassword = await _authRepository.CheckPasswordAsync(findUser, loginUser.Password);
            if (!checkPassword) throw new Exception("invalid Email or Password");
            if (!await _authRepository.IsEmailConfirmedAsync(findUser))
                throw new Exception("Email not confirmed");
            _ = await _authService.GenerateJwtToken(findUser);
            var refreshToken = _authService.GenerateRefreshToken(findUser.Id, loginUser.RememberMe);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return new UserResultDto()
            {
                Email = loginUser.Email,
                UserName = findUser.UserName,
                Token = await _authService.GenerateJwtToken(findUser),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpireOn
            };
        }
        public async Task<string> ConfirmEmail(string userId, string token)
        {
            var user = await _authRepository.FindByUserIdAsync(userId) ?? throw new Exception("User not found");
            _ = await _authRepository.ConfirmEmailAsync(user, token);

            return "Email confirmed successfully";
        }
        public async Task<string> ForgetPassword(string email)
        {
            await _authRepository.ForgetPasswordAsync(email);
            return "Email Has Sent Success";

        }
        public async Task<string> ResetPassword(ResetPasswordDto resetPassword)
        {
            var flag = await _authRepository.ResetPasswordAsync(resetPassword);
            if (!flag) return null;
            return "Password Changed Success";

        }
        public async Task<string> ConfirmEmailForAfterChanging(string userId, ConfirmChangeEmailRequest emailChange)
        {
            await _authRepository.ChangeEmailAsync(userId, emailChange);
            return "Email Has Changed Successfully";
        }
        public async Task<(string, RefreshToken)> RefreshToken(RefreshTokenRequest request)
        {
            var (accessToken, refreshToken) = await _authService.RefreshTokenAsync(request.RefreshToken);

            return (accessToken, refreshToken);
        }
        public async Task<string> LogoutAsync(string userId)
        {
            await _authRepository.LogoutAsync(userId);
            return "Logout Sucess";
        }
    }
}