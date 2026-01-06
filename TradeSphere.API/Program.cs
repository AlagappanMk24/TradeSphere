using KS_Sweets.Application.CrossCuttingConcerns.Logging;
using Microsoft.EntityFrameworkCore;
using TradeSphere.Infrastructure.Data.DbContext;

// =========================================================
// 1. WEB APPLICATION BUILDER
// =========================================================

var builder = WebApplication.CreateBuilder(args);

// Configure services for dependency injection
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

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
    // Get Configuration from app.Configuration
    string formattedDate = DateTime.Now.ToString("MM-dd-yyyy");
    string baseLogPath = app.Configuration.GetValue<string>("Logging:LogFilePath") ?? "Logs";
    string logFilePath = Path.Combine(baseLogPath, $"log-{formattedDate}.txt");

    // Get required services from the application service provider
    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();

    // Add the custom logging provider
    loggerFactory.AddProvider(new CustomFileLoggerProvider(logFilePath, httpContextAccessor));
}