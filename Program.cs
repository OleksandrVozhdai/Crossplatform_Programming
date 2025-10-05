using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.EntityFrameworkCore;

namespace disease_outbreaks_detector
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddDbContext<AppDbContext>(options =>
		        options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

			builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

			builder.Services.AddHttpClient();
			builder.Services.AddScoped<ExternalApi>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var externalApi = services.GetRequiredService<ExternalApi>();
				await externalApi.FetchAndStoreAsync("usa");
			}

			app.Run();

        }
    }
}
