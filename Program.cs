using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// === НАЛАШТУВАННЯ ENTITY FRAMEWORK З ДИНАМІЧНИМ ПРОВАЙДЕРОМ ===

// 1. Отримуємо назву провайдера з appsettings.json
var provider = builder.Configuration.GetValue<string>("Provider");

// 2. Реєструємо ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (provider)
    {
        case "SqlServer":
            // a. MS-SQL
            var sqlServerConStr = builder.Configuration.GetConnectionString("SqlServer");
            options.UseSqlServer(sqlServerConStr);
            Console.WriteLine("Using MS-SQL Server");
            break;

        case "Postgres":
            // b. Postgres
            var postgresConStr = builder.Configuration.GetConnectionString("Postgres");
            options.UseNpgsql(postgresConStr);
            Console.WriteLine("Using PostgreSQL");
            break;

        case "InMemory":
            // d. In-Memory
            options.UseInMemoryDatabase("InMemoryDb");
            Console.WriteLine("Using In-Memory Database");
            break;

        case "Sqlite":
        default:
            // c. SqlLite (за замовчуванням)
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

// Додаємо HttpClientFactory та іменуємо клієнт "default"
builder.Services.AddHttpClient("default");

builder.Services.AddScoped<ExternalApi>();

//Case Services
builder.Services.AddScoped<CaseRecordService>();


// === IDENTITY ===
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
        // ВИПРАВЛЕНО: Використовуємо builder.Configuration
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

var app = builder.Build();

// === MIDDLEWARE ===
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

// === ЗАПУСК МІГРАЦІЙ ТА НАПОВНЕННЯ БАЗИ (SEED) ===
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // Перевіряємо, чи база даних не є In-Memory, перш ніж застосовувати міграції
    if (provider != "InMemory")
    {
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Database migration applied.");
    }

    // Seed USA (Наповнення початковими даними)
    var externalApi = services.GetRequiredService<ExternalApi>();
    await externalApi.FetchAndStoreAsync("usa");
    Console.WriteLine("Seed USA — OK");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during DB migration or seeding: {ex.Message}");
}

app.Run();