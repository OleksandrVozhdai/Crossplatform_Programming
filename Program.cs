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

Console.WriteLine("App built successfully.");  // Log 1: Достигли build

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cases}/{action=Index}/{id?}");

Console.WriteLine("Pipeline configured.");  // Log 2: Достигли pipeline

// Ensure DB exists
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    Console.WriteLine("DB ensured created.");  // Log 3: БД готова
}

// Seed data (USA on startup)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var externalApi = services.GetRequiredService<ExternalApi>();
    try
    {
        Console.WriteLine("Seeding USA data...");  // Log 4
        await externalApi.FetchAndStoreAsync("usa");
        Console.WriteLine("Seed completed successfully.");  // Log 5
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seed failed: {ex.Message}");  // Log 6: Поймаем exception
    }
}

Console.WriteLine("Server starting...");  // Log 7: Перед Run
app.Run();  // Это держит сервер