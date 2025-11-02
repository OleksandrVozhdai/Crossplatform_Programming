using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===  ŒÕ‘≤√”–¿÷≤ﬂ ===
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=diseaseOutbreaksDB.db";

// === ¡¿«» ƒ¿Õ»’ ===
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// === —≈–¬≤—» ===
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ExternalApi>();

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
        options.ClientId = configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
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

// œŒ–ﬂƒŒ  ¬¿∆À»¬»…:
app.UseAuthentication();  // —ÔÓ˜‡ÚÍÛ Authentication
app.UseAuthorization();   // œÓÚ≥Ï Authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Index}/{id?}");

// === Ã≤√–¿÷≤Ø + SEED ===
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// COVID ¡ƒ
var caseDb = services.GetRequiredService<AppDbContext>();
caseDb.Database.Migrate();

// Identity ¡ƒ
var idDb = services.GetRequiredService<ApplicationDbContext>();
idDb.Database.Migrate();

// Seed USA
var externalApi = services.GetRequiredService<ExternalApi>();
try
{
    await externalApi.FetchAndStoreAsync("usa");
    Console.WriteLine("Seed USA ó OK");
}
catch (Exception ex)
{
    Console.WriteLine($"Seed error: {ex.Message}");
}

app.Run();