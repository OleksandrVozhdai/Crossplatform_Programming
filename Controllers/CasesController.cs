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
    }
}