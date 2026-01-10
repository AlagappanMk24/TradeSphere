using TradeSphere.Infrastructure.Repositories.OrderRepository;

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
            service.AddAutoMapper(cfg => { }, typeof(CategoryProfile).Assembly);
            service.AddAutoMapper(cfg => { }, typeof(ProductProfile).Assembly);

            service.AddScoped<IAuthUseCase, AuthUseCase>();
            service.AddScoped<IAccountUseCase, AccountUseCase>();
            service.AddScoped<IRoleUseCase, RoleUseCase>();
            service.AddScoped<ICategoryUseCase, CategoryUseCase>();
            service.AddScoped<IProductUseCase, ProductUseCase>();
            service.AddScoped<IShoppingCartUseCase, ShoppingCartUseCase>();
            service.AddScoped<IOrderUseCase, OrderUseCase>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IAuthRepository, AuthRepository>();
            service.AddScoped<IAccountRepository, AccountRepository>();
            service.AddScoped<IRoleRepository, RoleRepository>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            service.AddScoped<IOrderRepository, OrderRepository>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            //service.AddAutoMapper(cfg => { }, typeof(CategoryProfile).Assembly);
            return service;
        }
    }
}
