using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace disease_outbreaks_detector.Controllers
{
	public class CountriesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CountriesController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(string filterString)
		{

			var countries = await _context.CaseRecords
				.Select(c => new CountryViewModel
				{
					Id = c.Id,
					Name = c.Country
				})
				.Where(c => string.IsNullOrEmpty(filterString) || c.Name.StartsWith(filterString) || c.Name.EndsWith(filterString))
				.ToListAsync();

			ViewBag.Filter = filterString;

			return View(countries);
		}

		public async Task<IActionResult> Details(string SelectedCountry)
		{
			CaseRecord? record = null;
			int deathDiff = 0, recoveredDiff = 0;

			try
			{
				record = await _context.CaseRecords
					.FirstOrDefaultAsync(r => r.Country == SelectedCountry);

				using var client = new HttpClient();
				var escapedCountry = Uri.EscapeDataString(SelectedCountry);

				var url = $"https://localhost:7147/api/CaseRecord/Name/{SelectedCountry}";

				var response = await client.GetAsync(url);
				if (!response.IsSuccessStatusCode)
				{
					Console.WriteLine($"API call failed: {response.StatusCode}");
				}
				else
				{
					var json = await response.Content.ReadAsStringAsync();
					using var doc = JsonDocument.Parse(json);
					var root = doc.RootElement;

					if (root.TryGetProperty("timeline", out var timeline))
					{
						if (timeline.TryGetProperty("deaths", out var deaths) &&
							deaths.EnumerateObject().Any())
						{
							var arr = deaths.EnumerateObject().ToArray();
							var firstDeath = arr.First().Value.GetInt32();
							var lastDeath = arr.Last().Value.GetInt32();
							deathDiff = lastDeath - firstDeath;
						}

						if (timeline.TryGetProperty("recovered", out var recovered) &&
							recovered.EnumerateObject().Any())
						{
							var arr = recovered.EnumerateObject().ToArray();
							var firstRec = arr.First().Value.GetInt32();
							var lastRec = arr.Last().Value.GetInt32();
							recoveredDiff = lastRec - firstRec;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"Error fetching data for {SelectedCountry}: {ex.Message}";
			}

			ViewBag.Country = SelectedCountry;
			ViewBag.Record = record;
			ViewBag.Death = deathDiff;
			ViewBag.Recovered = recoveredDiff;

			return View(record);
		}

	}
}
