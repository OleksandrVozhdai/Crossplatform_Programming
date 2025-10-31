using Microsoft.AspNetCore.Mvc;
using disease_outbreaks_detector.Services;
using disease_outbreaks_detector.Models;

namespace disease_outbreaks_detector.Controllers
{
    public class CasesController : Controller
    {
        private readonly ExternalApi _externalApi;

        public CasesController(ExternalApi externalApi)
        {
            _externalApi = externalApi;
        }

        public async Task<IActionResult> Index(string country = "USA")
        {
            CaseRecord? record = null;
            try
            {
                record = await _externalApi.FetchAndStoreAsync(country);
                Console.WriteLine($"Fetched record for {country}: {record != null}");  // Debug log
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fetch error for {country}: {ex.Message}");  // Catch crash
                ViewBag.Error = $"Error fetching data for {country}: {ex.Message}";
            }

            if (record == null)
            {
                ViewBag.Error = $"No data for {country}. Check console for API error.";
            }
            else
            {
                ViewBag.Country = country;
                ViewBag.Record = record;
            }
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