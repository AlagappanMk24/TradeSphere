namespace TradeSphere.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    // [Authorize("Admin")]
    public class RoleController(RoleUseCase roleUseCase) : ControllerBase
    {
        private readonly IRoleUseCase _roleUseCase = roleUseCase;

        [HttpGet] // GET api/roles
        public async Task<ActionResult<RoleDto>> GetAllRoles()
        {
            var roles = await _roleUseCase.GetAllRoles();
            if (roles is null) return NotFound("No roles found.");
            return Ok(roles);
        }

        [HttpGet("{id:int}")] // GET api/roles/5
        public async Task<ActionResult<RoleDto>> GetRoleById(int id)
        {
            var role = await _roleUseCase.GetRoleById(id);
            if (role is null) return NotFound($"Role with ID {id} not found.");
            return Ok(role);
        }

        // Standard for relationship: resources/id/sub-resource
        [HttpGet("users/{userId}")] // GET api/roles/users/1
        public async Task<ActionResult<RoleDto>> GetUserRole(int userId)
        {
            var role = await _roleUseCase.GetUserRole(userId);
            if (role is null) return NotFound($"Role for User ID {userId} not found.");
            return Ok(role);
        }

        [HttpPut("{roleId:int}")] // PUT api/roles/5
        public async Task<ActionResult> UpdateRole(int roleId, [FromBody] string name)
        {
            var result = await _roleUseCase.UpdateRole(roleId, name);
            if (!result) return NotFound($"Role with ID {roleId} not found.");
            return NoContent();
        }

        [HttpPost] // POST api/roles
        public async Task<ActionResult> AddRole([FromBody] string roleName)
        {
            var result = await _roleUseCase.AddRole(roleName);
            if (result == "Role name cannot be empty")
                return BadRequest(new ApiResponse(400, result));
            return Ok(new ApiResponse(200, result));
        }

        [HttpPost("assignments")] // POST api/roles/assignments
        public async Task<ActionResult> ChangeUserRole([FromBody] UserRoleChangeDto dto)
        {
            var result = await _roleUseCase.ChangeUserRole(dto.UserId, dto.RoleName);
            if (!result) return NotFound($"Failed to change role for User ID {dto.UserId}.");
            return NoContent();
        }
    }
}