namespace TradeSphere.Application.Mappings.RoleMapping
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<ApplicationRole, RoleDto>();
        }
    }
}