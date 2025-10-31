using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ExternalApi>(provider =>
    new ExternalApi(
        provider.GetRequiredService<AppDbContext>(),
        provider.GetRequiredService<IHttpClientFactory>()
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();
//app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Index}");

// Ensure DB exists and create table
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    Console.WriteLine("DB table created.");
}

// Seed data (USA on startup)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var externalApi = services.GetRequiredService<ExternalApi>();
    try
    {
        Console.WriteLine("Seeding USA data...");
        await externalApi.FetchAndStoreAsync("usa");
        Console.WriteLine("Seed completed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seed failed: {ex.Message}");
    }
}

Console.WriteLine("Server starting on http://localhost:5052");
app.Run();