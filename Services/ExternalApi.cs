using disease_outbreaks_detector.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace disease_outbreaks_detector.Services
{
    //This API we took from:
    //https://disease.sh/docs/#/COVID-19%3A%20Worldometers/get_v3_covid_19_countries__country_
    //

    public class ExternalApi
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public ExternalApi(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<CaseRecord?> FetchAndStoreAsync(string country)
        {
            var url = $"https://disease.sh/v3/covid-19/countries/{country}?yesterday=true&strict=true";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API call failed: {response.StatusCode}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<CaseRecord>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data == null)
            {
                Console.WriteLine($"Deserialization failed for {country}: Check JSON format.");  // Debug
                return null;
            }

            // Parse lat/long from countryInfo
            using JsonDocument doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("countryInfo", out var countryInfo))
            {
                data.Latitude = countryInfo.TryGetProperty("lat", out var latProp) ? latProp.GetDouble() : null;
                data.Longitude = countryInfo.TryGetProperty("long", out var longProp) ? longProp.GetDouble() : null;
            }

            bool exists = await _context.CaseRecords.AnyAsync(x => x.Country.ToLower() == data.Country.ToLower());
            if (!exists)
            {
                _context.CaseRecords.Add(data);
                await _context.SaveChangesAsync();
            }
            else
            {
                var existing = await _context.CaseRecords.FirstOrDefaultAsync(x => x.Country.ToLower() == data.Country.ToLower());
                if (existing != null)
                {
                    existing.Cases = data.Cases;
                    existing.TodayCases = data.TodayCases;
                    existing.Deaths = data.Deaths;
                    existing.TodayDeaths = data.TodayDeaths;
                    existing.Recovered = data.Recovered;
                    existing.TodayRecovered = data.TodayRecovered;
                    existing.population = data.population;
                    existing.UpdatedAt = data.UpdatedAt;
                    existing.Active = data.Active;
                    existing.Critical = data.Critical;
                    existing.Latitude = data.Latitude;
                    existing.Longitude = data.Longitude;
                    await _context.SaveChangesAsync();
                }
            }

            return data;
        }
    }
}