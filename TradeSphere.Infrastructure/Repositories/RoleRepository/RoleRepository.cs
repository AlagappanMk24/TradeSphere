namespace TradeSphere.Infrastructure.Repositories.RoleRepository
{
    public class RoleRepository(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager) : IRoleRepository
    {
        public async Task<string> AddRole(string roleName)
        {
            var existingRole = await roleManager.FindByNameAsync(roleName);
            if (existingRole != null)
                throw new Exception("Role already exists");
            var role = new ApplicationRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));
            return "Role Created Successfully";
        }
        public async Task<bool> ChangeUserRole(int userId, string roleName)
        {
            var findUser = await userManager.FindByIdAsync(userId.ToString());
            if (findUser is null) throw new Exception("User not found");
            var roles = await userManager.GetRolesAsync(findUser);
            var removeRole = await userManager.RemoveFromRolesAsync(findUser, roles);
            if (!removeRole.Succeeded) throw new Exception("Failed to remove existing roles");
            var addRole = await userManager.AddToRoleAsync(findUser, roleName);
            if (!addRole.Succeeded) throw new Exception("Failed to add new role");
            return true;
        }
        public async Task<List<ApplicationRole>> GetAllRoles() => await roleManager.Roles.ToListAsync();
        public Task<ApplicationRole> GetRoleById(int roleId) => roleManager.FindByIdAsync(roleId.ToString())!;
        public async Task<ApplicationRole> GetUserRole(int UserId)
        {
            var findUser = await userManager.FindByIdAsync(UserId.ToString());
            if (findUser is null) throw new Exception("User not found");
            var roles = await userManager.GetRolesAsync(findUser);
            var userRole = await roleManager.FindByNameAsync(roles.FirstOrDefault()!);
            return userRole!;
        }
        public async Task<bool> UpdateRole(int roleId, string name)
        {

            var role = await roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
                return false;


            var existingRole = await roleManager.FindByNameAsync(name);
            if (existingRole != null && existingRole.Id != roleId)
                throw new Exception("Role name already exists");


            role.Name = name;
            role.NormalizedName = name.ToUpper();


            var result = await roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ",
                    result.Errors.Select(e => e.Description)));

            return true;
        }
    }
}