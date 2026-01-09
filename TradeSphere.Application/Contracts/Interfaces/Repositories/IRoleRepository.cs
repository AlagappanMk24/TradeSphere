namespace TradeSphere.Application.Contracts.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        public Task<List<ApplicationRole>> GetAllRoles();
        public Task<ApplicationRole> GetRoleById(int roleId);
        public Task<ApplicationRole> GetUserRole(int UserId);
        public Task<bool> UpdateRole(int roleId, string name);
        public Task<string> AddRole(string roleName);
        public Task<bool> ChangeUserRole(int userId, string roleName);
    }
}