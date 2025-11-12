using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var provider = builder.Configuration.GetValue<string>("Provider");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (provider)
    {
        case "SqlServer":

            var sqlServerConStr = builder.Configuration.GetConnectionString("SqlServer");
            options.UseSqlServer(sqlServerConStr);
            Console.WriteLine("Using MS-SQL Server");
            break;

        case "Postgres":

            var postgresConStr = builder.Configuration.GetConnectionString("Postgres");
            options.UseNpgsql(postgresConStr);
            Console.WriteLine("Using PostgreSQL");
            break;

        case "InMemory":

            options.UseInMemoryDatabase("InMemoryDb");
            Console.WriteLine("Using In-Memory Database");
            break;

        case "Sqlite":
        default:

            var sqliteConStr = builder.Configuration.GetConnectionString("Sqlite")
                               ?? "Data Source=diseaseOutbreaksDB.db";
            options.UseSqlite(sqliteConStr);
            Console.WriteLine("Using SQLite");
            break;
    }
});

// ===============================================================


builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient("default");

builder.Services.AddScoped<ExternalApi>();

builder.Services.AddScoped<CaseRecordService>();


builder.Services.AddIdentity<AppDbContextUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// === GOOGLE OAUTH2 ===
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Index}/{id?}");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    if (provider != "InMemory")
    {
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Database migration applied.");
    }

    var externalApi = services.GetRequiredService<ExternalApi>();
    await externalApi.FetchAndStoreAsync("usa");
    Console.WriteLine("Seed USA — OK");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during DB migration or seeding: {ex.Message}");
}

app.Run();