using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace disease_outbreaks_detector.Controllers
{
    public class CasesController : Controller
    {
        private readonly ExternalApi _externalApi;
		private readonly HttpClient _httpClient;

		public CasesController(ExternalApi externalApi)
        {
            _externalApi = externalApi;
        }

		public async Task<IActionResult> Index(string country = "USA", string? numberOfDays = null)
		{
			CaseRecord? record = null;
			int deathDiff = 0, recoveredDiff = 0;

			try
			{
				if (string.IsNullOrWhiteSpace(numberOfDays))
				{
					record = await _externalApi.FetchAndStoreAsync(country);
				}
				else
				{
					record = await _externalApi.FetchAndStoreAsync(country);

					using var client = new HttpClient();
					var url = $"https://disease.sh/v3/covid-19/historical/{country}?lastdays={numberOfDays}";
					var response = await client.GetAsync(url);

					if (!response.IsSuccessStatusCode)
					{
						Console.WriteLine($"API call failed: {response.StatusCode}");
						return View();
					}

					var json = await response.Content.ReadAsStringAsync();

					using var doc = JsonDocument.Parse(json);
					var root = doc.RootElement;

					var deaths = root.GetProperty("timeline").GetProperty("deaths");
					var recovered = root.GetProperty("timeline").GetProperty("recovered");

					var firstDeath = deaths.EnumerateObject().First().Value.GetInt32();
					var lastDeath = deaths.EnumerateObject().Last().Value.GetInt32();
					var firstRecovered = recovered.EnumerateObject().First().Value.GetInt32();
					var lastRecovered = recovered.EnumerateObject().Last().Value.GetInt32();

					deathDiff = lastDeath - firstDeath;
					recoveredDiff = lastRecovered - firstRecovered;
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"Error fetching data for {country}: {ex.Message}";
			}

			ViewBag.Country = country;
			ViewBag.Record = record;
			ViewBag.Death = deathDiff;
			ViewBag.Recovered = recoveredDiff;
			ViewBag.NumberOfDays = numberOfDays;

			return View();
		}


		public async Task<IActionResult> ChoosePage()
		{
			return View();
		}


		public async Task<IActionResult> CompareCountries(string country1 = "USA", string country2 = "Canada")
		{
			CaseRecord? record1 = null;
			CaseRecord? record2 = null;

			try
			{
				record1 = await _externalApi.FetchAndStoreAsync(country1);
				record2 = await _externalApi.FetchAndStoreAsync(country2);
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"Error comparing countries: {ex.Message}";
			}

			ViewBag.Country1 = country1;
			ViewBag.Country2 = country2;
			ViewBag.Record1 = record1;
			ViewBag.Record2 = record2;

			return View();
		}
	}
}