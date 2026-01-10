namespace TradeSphere.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController(IAccountUseCase accountUseCase) : ControllerBase
    {
        private readonly IAccountUseCase _accountUseCase = accountUseCase;

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var result = await _accountUseCase.GetProfile(userId);
            if (result is null)
                return BadRequest(new ApiResponse(400, "Try Again"));
            return Ok(result);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();
            var result = await _accountUseCase.UpdateProfile(userId, updateProfile);
            if (result is null)
                return BadRequest(new ApiResponse(400, "Try Again"));
            return Ok(result);
        }

        [HttpPost("password/change")]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null)
                return Unauthorized();

            var result = await _accountUseCase.ChangePassword(userEmail, currentPassword, newPassword);
            if (string.IsNullOrEmpty(result))
                return BadRequest(new ApiResponse(400, "Try Again"));

            return Ok(new ApiResponse(200, result));
        }

        [HttpPost("email/change-request")]
        public async Task<IActionResult> ChangeEmail(string newEmail)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var result = await _accountUseCase.RequestChangeEmail(userEmail, newEmail);
            if (string.IsNullOrEmpty(result))
                return BadRequest(new ApiResponse(400, "InvalidOperation"));

            return Ok(new ApiResponse(200, result));
        }
    }
}