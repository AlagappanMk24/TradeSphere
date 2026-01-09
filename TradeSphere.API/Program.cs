// =========================================================
// WEB APPLICATION BUILDER
// =========================================================
using Microsoft.Extensions.Options;
using TradeSphere.Application.Settings;

var builder = WebApplication.CreateBuilder(args);

// 1. Service Configuration
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// 2. Database Initialization
await app.ApplyMigrationWithSeed();

// =========================================================
// 2. MIDDLEWARE CONFIGURATION
// =========================================================
ConfigureMiddleware(app);

app.Run();

// =========================================================
// 3. SERVICE CONFIGURATION (ADD SERVICES)
// =========================================================
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // --- Infrastructure Services (DB, Identity, DI Container) ---

    // Configure Database Context
    ConfigureDatabase(services, configuration);

    // Web API Core Services
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddHttpContextAccessor();

    // Custom Extension Methods
    services.ApplyServices(builder.Configuration);
    services.AddAuthorizeSwaggerAsync(builder.Configuration);
    services.ValidationResponse();

    services.Configure<FileLoggingSettings>(configuration.GetSection(FileLoggingSettings.SectionName));
    services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
}

// ====================== Database Connection ======================
void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("TradeSphereDbConnection");

    // Check for missing connection string in a robust way
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'TradeSphereDbConnection' not found in configuration.");
    }
    // Add DbContext with SQL Server
    services.AddDbContext<TradeSphereDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// =========================================================
// 4. MIDDLEWARE PIPELINE CONFIGURATION (USE SERVICES)
// =========================================================
void ConfigureMiddleware(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseMiddleware<GlobalErrorMiddleware>();

    // Redirect HTTP to HTTPS
    app.UseHttpsRedirection();

    // Serve static files (CSS, JS, images)
    app.UseStaticFiles();

    // Authentication must run before Authorization
    app.UseAuthentication();

    // Enable authorization
    app.UseAuthorization();

    // Configure Custom Logging (must run after app.Build() to access services)
    ConfigureCustomLogging(app);

    app.MapControllers();
}
void ConfigureCustomLogging(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    // Get the settings using IOptions
    var settings = scope.ServiceProvider.GetRequiredService<IOptions<FileLoggingSettings>>().Value;

    string logFilePath = Path.Combine(settings.LogFilePath, $"log-{DateTime.Now:MM-dd-yyyy}.txt");

    // Get required services from the application service provider
    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();

    // Add the custom logging provider
    loggerFactory.AddProvider(new CustomFileLoggerProvider(logFilePath, httpContextAccessor));
}