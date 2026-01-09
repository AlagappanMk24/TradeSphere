namespace TradeSphere.Application.UseCases.Interfaces
{
    public interface IRoleUseCase
    {
        Task<List<RoleDto>> GetAllRoles();
        Task<RoleDto> GetRoleById(int roleId);
        Task<RoleDto> GetUserRole(int userId);
        Task<bool> UpdateRole(int roleId, string name);
        Task<string> AddRole(string roleName);
        Task<bool> ChangeUserRole(int userId, string roleName);
    }
}
