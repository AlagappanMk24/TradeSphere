namespace TradeSphere.Application.UseCases
{
    public class AccountUseCase(IAccountRepository accountRepository, IMapper mapper, IAuthRepository authRepository) : IAccountUseCase
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuthRepository _authRepository = authRepository;
        public async Task<UserProfileDto> GetProfile(string UserId)
        {
            var user = await _accountRepository.GetProfileAsync(UserId) ?? throw new Exception("User Not Found");
            return _mapper.Map<UserProfileDto>(user); ;
        }
        public async Task<UserProfileDto> UpdateProfile(string UserId, UpdateProfileDto updateProfile)
        {
            var user = await _accountRepository.GetProfileAsync(UserId) ?? throw new Exception("User Not Found");
            var updatedUser = _mapper.Map(updateProfile, user);
            var result = await _accountRepository.UpdateProfileAsync(updatedUser);
            return _mapper.Map<UserProfileDto>(result);

        }
        public async Task<string> ChangePassword(string email, string currentPassword, string newPassword)
        {
            var user = await _authRepository.FindByEmailAsync(email);
            _ = await _accountRepository.ChangePasswordAsync(user, currentPassword, newPassword);
            return "PasswordChangeSucess";

        }
        public async Task<string> RequestChangeEmail(string currentEmail, string newEmail)
        {
            await _accountRepository.RequestChangeEmailAsync(currentEmail, newEmail);
            return "Email Has Sent Success";
        }
    }
}