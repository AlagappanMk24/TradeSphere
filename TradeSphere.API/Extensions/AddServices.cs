using TradeSphere.Application.Mappings.AccountMapping;
using TradeSphere.Application.Mappings.RoleMapping;
using TradeSphere.Infrastructure.Repositories.RoleRepository;

namespace TradeSphere.Api.Extensions
{
    public static class AddServices
    {
        public static IServiceCollection ApplyServices(this IServiceCollection service, IConfiguration configration)
        {
            service.AddDbContext<TradeSphereDbContext>(opt =>
            {
                opt.UseSqlServer(configration.GetConnectionString("conn1"));
            });

            service.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<TradeSphereDbContext>().AddDefaultTokenProviders();

            service.AddAutoMapper(cfg => { }, typeof(AccountProfile).Assembly);
            service.AddAutoMapper(cfg => { }, typeof(RoleProfile).Assembly);

            service.AddScoped<IAuthUseCase, AuthUseCase>();
            service.AddScoped<IAccountUseCase, AccountUseCase>();
            service.AddScoped<IRoleUseCase, RoleUseCase>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IAuthRepository, AuthRepository>();
            service.AddScoped<IAccountRepository, AccountRepository>();
            service.AddScoped<IRoleRepository, RoleRepository>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            //service.AddAutoMapper(cfg => { }, typeof(CategoryProfile).Assembly);
            return service;
        }
    }
}
