using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

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
builder.Services.AddSwaggerGen(options =>
{

	var provider = builder.Services.BuildServiceProvider()
					 .GetRequiredService<IApiVersionDescriptionProvider>();

	
	foreach (var description in provider.ApiVersionDescriptions)
	{
		options.SwaggerDoc(description.GroupName, new OpenApiInfo
		{
			Title = $"Disease Outbreaks Detector API {description.ApiVersion}",
			Version = description.ApiVersion.ToString(),
		});
	}
});


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

builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(2, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
});


builder.Services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});



// === GOOGLE OAUTH2 ===
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });


// ZIPKIN + TELEMETRY

// ======================= OPENTELEMETRY + ZIPKIN ==========================
builder.Services.AddOpenTelemetry()
	.WithTracing(tracing =>
	{
		tracing
			.AddAspNetCoreInstrumentation()
			.AddHttpClientInstrumentation()
			.AddEntityFrameworkCoreInstrumentation()
			.AddSource("DiseaseOutbreaks")
			.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("DiseaseOutbreaksAPI"))
			.AddZipkinExporter();
	})
	.WithMetrics(metrics =>
	{
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation();
	});

var app = builder.Build();

var providerDesc = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
	foreach (var description in providerDesc.ApiVersionDescriptions)
	{
		options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
								$"API {description.GroupName.ToUpperInvariant()}");
	}
});
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