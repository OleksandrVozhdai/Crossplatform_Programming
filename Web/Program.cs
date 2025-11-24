using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.AspNetCore.Mvc.Versioning; // Adding using for ApiVersionRouteConstraint
using Microsoft.AspNetCore.Routing; // Adding using for RouteOptions

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// === DATABASE ===
var provider = config["Database:Provider"] ?? "Sqlite";
var connectionString = config.GetConnectionString(provider) ?? "Data Source=diseaseOutbreaksDB.db";

// === APPLICATION DB CONTEXT ===
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (provider)
    {
        case "SqlServer": options.UseSqlServer(connectionString); break;
        case "Postgres": options.UseNpgsql(connectionString); break;
        case "Sqlite":
        default: options.UseSqlite(connectionString); break;
    }
});

// === IDENTITY + RAZOR PAGES ===
builder.Services.AddIdentity<AppDbContextUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages(); // ← Required!

// === DUENDE IDENTITY SERVER ===
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential() // dev only
    .AddAspNetIdentity<AppDbContextUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlite(connectionString, sql =>
                sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlite(connectionString, sql =>
                sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
        options.EnableTokenCleanup = true;
    });

// === GOOGLE ===
builder.Services.AddAuthentication()
    .AddGoogle("Google", options =>
    {
        options.ClientId = config["Authentication:Google:ClientId"]!;
        options.ClientSecret = config["Authentication:Google:ClientSecret"]!;
    });

// === API + SWAGGER + VERSIONING (UPDATED BLOCK) ===
builder.Services.AddControllersWithViews();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// REMOVED: The following block was removed because AddApiVersioning already registers 
// the "apiVersion" constraint, causing a duplicate key exception.
// builder.Services.Configure<RouteOptions>(options =>
// {
//     options.ConstraintMap["apiVersion"] = typeof(ApiVersionRouteConstraint);
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Keeping the rest of the services from the original block
builder.Services.AddHttpClient();
builder.Services.AddScoped<ExternalApi>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();         // ← BEFORE UseAuthentication!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Index}/{id?}");

app.MapRazorPages(); // ← Login/Registration pages

app.Run();