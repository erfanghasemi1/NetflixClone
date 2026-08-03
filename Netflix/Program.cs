using Microsoft.EntityFrameworkCore;
using Netflix.Configurations;
using Netflix.Data;
using Netflix.Services.Implementations;
using Netflix.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------
// Controllers
// ---------------------------------
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ---------------------------------
// Database
// ---------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------------
// Options
// ---------------------------------
builder.Services.Configure<TmdbOptions>(
    builder.Configuration.GetSection(TmdbOptions.SectionName));

builder.Services.Configure<ImportOptions>(
    builder.Configuration.GetSection(ImportOptions.SectionName));

// ---------------------------------
// HttpClient + Services
// ---------------------------------
builder.Services.AddHttpClient<ITmdbService, TmdbService>();

builder.Services.AddScoped<IGenreImportService, GenreImportService>();
builder.Services.AddScoped<IMediaImportService, MediaImportService>();
builder.Services.AddScoped<IImportOrchestrator, ImportOrchestrator>();
builder.Services.AddScoped<IGetMediaSample, GetMediaSample>();
builder.Services.AddScoped<ISearchMedia, SearchMedia>();

var app = builder.Build();

// ---------------------------------
// Pipeline
// ---------------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


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