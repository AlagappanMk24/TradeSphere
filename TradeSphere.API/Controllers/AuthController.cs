namespace TradeSphere.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")] // Added versioning
    public class AuthController(IAuthUseCase authUseCase) : ControllerBase
    {
        private readonly IAuthUseCase _authUseCase = authUseCase;

        [HttpPost("login")]
        public async Task<ActionResult<UserResultDto>> Login([FromBody] UserLoginDto user)
        {
            var result = await _authUseCase.LoginUser(user);
            if (result is null) return BadRequest(new ApiResponse(400, message: "Invalid Email Or Password"));
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResultDto>> Register([FromBody] UserRegisterDto user)
        {
            var result = await _authUseCase.RegisterUser(user);
            return Ok(result);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _authUseCase.ConfirmEmail(userId, token);
            return Ok(new ApiResponse(200, result));
        }

        [HttpGet("confirm-email-change")] 
        public async Task<IActionResult> ConfirmChangeEmail([FromQuery] string userId, [FromQuery] string newEmail, [FromQuery] string token)
        {
            var result = await _authUseCase.ConfirmEmailForAfterChanging(
                userId,
                new ConfirmChangeEmailRequest
                {
                    NewEmail = newEmail,
                    Token = token
                });

            if (string.IsNullOrEmpty(result))
                return BadRequest(new ApiResponse(400, "Invalid or expired token"));

            return Ok(new ApiResponse(200, result));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromQuery] string email)
        {
            await _authUseCase.ForgetPassword(email);
            return NoContent();
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            var result = await _authUseCase.ResetPassword(resetPassword);
            if (string.IsNullOrEmpty(result))
                return BadRequest(new ApiResponse(400, "Try Again"));

            return Ok(new ApiResponse(200, result));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var (accessToken, refreshToken) = await _authUseCase.RefreshToken(request);

            return Ok(new UserResultDto
            {
                Token = accessToken,
                RefreshTokenExpiration = refreshToken.ExpireOn
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var result = await _authUseCase.LogoutAsync(userId);
            return Ok(new ApiResponse(200, result));
        }
    }
}