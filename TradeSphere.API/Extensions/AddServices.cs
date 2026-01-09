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

            service.AddScoped<IAuthUseCase, AuthUseCase>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IAuthRepository, AuthRepository>();
            service.AddScoped<IAccountRepository, AccountRepository>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            //service.AddAutoMapper(cfg => { }, typeof(CategoryProfile).Assembly);
            return service;
        }
    }
}
