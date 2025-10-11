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
            _httpClient = httpClientFactory.CreateClient("default");  // Named overload, mockable
        }

        public async Task<CaseRecord?> FetchAndStoreAsync(string country)
        {
            var url = $"https://disease.sh/v3/covid-19/countries/{country}?yesterday=true&strict=true";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<CaseRecord>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data == null)
                return null;

            bool exists = await _context.CaseRecords.AnyAsync(x => x.Country == data.Country);
            if (!exists)
            {
                _context.CaseRecords.Add(data);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Update existing (simple, add more fields if needed)
                var existing = await _context.CaseRecords.FirstOrDefaultAsync(x => x.Country == data.Country);
                if (existing != null)
                {
                    existing.Cases = data.Cases;
                    existing.Deaths = data.Deaths;
                    // Add other fields
                    existing.UpdatedAt = data.UpdatedAt;
                    await _context.SaveChangesAsync();
                    return data;  // Добавь в конец
                }
            }

            return data;
        }
    }
}