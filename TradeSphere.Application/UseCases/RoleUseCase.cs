namespace TradeSphere.Application.UseCases
{
    public class RoleUseCase(IMapper mapper, IRoleRepository roleRepository) : IRoleUseCase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRoleRepository _roleRepository = roleRepository;
        public async Task<List<RoleDto>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRoles();
            if (roles is null) return null;
            return _mapper.Map<List<RoleDto>>(roles);
        }
        public async Task<RoleDto> GetRoleById(int roleId)
        {
            var role = await _roleRepository.GetRoleById(roleId);
            if (role is null) return null;
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<RoleDto> GetUserRole(int userId)
        {
            var role = await _roleRepository.GetUserRole(userId);
            if (role is null) return null;
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<bool> UpdateRole(int roleId, string name)
        {
            return await _roleRepository.UpdateRole(roleId, name);
        }
        public async Task<string> AddRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return "Role name cannot be empty";
            return await _roleRepository.AddRole(roleName);
        }
        public async Task<bool> ChangeUserRole(int userId, string roleName)
        {
            return await _roleRepository.ChangeUserRole(userId, roleName);
        }
    }
}