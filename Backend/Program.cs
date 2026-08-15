using Microsoft.EntityFrameworkCore;
using Netflix.Configurations;
using Netflix.Data;
using Netflix.Services.Implementations;
using Netflix.Services.Interfaces;

// =============================================================================
// Program.cs - Application Entry Point
// Configures and bootstraps the ASP.NET Core Web API application including
// services, middleware pipeline, database migration, and dependency injection.
// =============================================================================

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------
// Controllers
// ---------------------------------
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ---------------------------------
// Database
// Registers the Entity Framework Core DbContext with SQL Server using
// the connection string defined in appsettings.json.
// ---------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------------
// Options
// Binds strongly-typed configuration sections from appsettings.json
// to their corresponding options classes for use via IOptions<T>.
// ---------------------------------
builder.Services.Configure<TmdbOptions>(
    builder.Configuration.GetSection(TmdbOptions.SectionName));

builder.Services.Configure<ImportOptions>(
    builder.Configuration.GetSection(ImportOptions.SectionName));

// ---------------------------------
// HttpClient + Services
// Registers the typed HttpClient for TMDB API communication and
// all scoped application services with their interfaces.
// ---------------------------------
builder.Services.AddHttpClient<ITmdbService, TmdbService>();

builder.Services.AddScoped<IGenreImportService, GenreImportService>();
builder.Services.AddScoped<IMediaImportService, MediaImportService>();
builder.Services.AddScoped<IImportOrchestrator, ImportOrchestrator>();
builder.Services.AddScoped<ISearchMedia, SearchMedia>();

var app = builder.Build();

// ---------------------------------
// Pipeline
// Configures the HTTP request middleware pipeline including OpenAPI,
// HTTPS redirection, authorization, and controller routing.
// ---------------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ---------------------------------
// Database Migration
// Automatically applies any pending Entity Framework Core migrations
// on startup, logging any errors that occur during the process.
// ---------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // This applies all pending migrations to the database
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();