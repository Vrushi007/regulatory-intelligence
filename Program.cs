using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RimPoc.Data;
using RimPoc.Services;
using RimPoc.Tools;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = Host.CreateApplicationBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add logging
builder.Services.AddLogging(configure => configure.AddConsole());

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JSON serialization to handle reference loops
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddMcpServer().WithStdioServerTransport()
    .WithTools<CountryTools>()
    .WithTools<ProductTools>()
    .WithTools<ProductFamilyTools>()
    .WithTools<ApplicationTools>()
    .WithTools<SubmissionTools>()
    .WithTools<ControlledVocabularyTools>()
    .WithTools<DefaultTemplateContentTools>()
    .WithTools<PlanTools>();

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});
// Add services
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductFamilyService, ProductFamilyService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IControlledVocabularyService, ControlledVocabularyService>();
builder.Services.AddScoped<DefaultTemplateContentService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<PlanService>();
builder.Services.AddScoped<DataSeederService>();
var host = builder.Build();

// Ensure database is created and seed initial data
await InitializeDatabaseAsync(host);

// Run the host (this will start the MCP server)
await host.RunAsync();

static async Task InitializeDatabaseAsync(IHost host)
{
    using var scope = host.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
    var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();
    var dataSeederService = scope.ServiceProvider.GetRequiredService<DataSeederService>();

    try
    {
        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Database is ready!");

        // Seed initial data from JSON files
        logger.LogInformation("Starting data seeding process...");
        await dataSeederService.SeedDataAsync();

        logger.LogInformation("Database initialization completed. Ready for user operations.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while initializing the database");
    }
}


