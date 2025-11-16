using disease_outbreaks_detector.Models; 
using Microsoft.AspNetCore.Mvc;

namespace disease_outbreaks_detector.Controllers
{
    public class CentralController : Controller
    {

        private static List<CaseRecord> _mockDatabase = new List<CaseRecord>
        {
            new CaseRecord
            {
                Id = 1,
                Country = "Ukraine",
                Cases = 150000,
                TodayCases = 120,
                Deaths = 3000,
                TodayDeaths = 5,
                Recovered = 120000,
                TodayRecovered = 200,
                population = 41000000,
                Active = 27000,
                Critical = 150,
                UpdatedAt = DateTime.Now.AddDays(-1)
            },
            new CaseRecord
            {
                Id = 2,
                Country = "Poland",
                Cases = 280000,
                TodayCases = 300,
                Deaths = 5000,
                TodayDeaths = 12,
                Recovered = 250000,
                TodayRecovered = 400,
                population = 38000000,
                Active = 25000,
                Critical = 110,
                UpdatedAt = DateTime.Now.AddDays(-2)
            },
            new CaseRecord
            {
                Id = 3,
                Country = "Germany",
                Cases = 500000,
                TodayCases = 550,
                Deaths = 11000,
                TodayDeaths = 20,
                Recovered = 450000,
                TodayRecovered = 600,
                population = 83000000,
                Active = 39000,
                Critical = 200,
                UpdatedAt = DateTime.Now.AddDays(-1)
            }
        };


        // ЗАВДАННЯ 1: СТОРІНКА-СПИСОК (List page)
        public IActionResult Index()
        {
            var allCases = _mockDatabase;
            return View(allCases); 
        }

        // ЗАВДАННЯ 2: СТОРІНКА-ДЕТАЛІ (Element-by-element page)
        public IActionResult Details(int id)
        {
            var singleRecord = _mockDatabase.FirstOrDefault(record => record.Id == id);

            if (singleRecord == null)
            {
                return NotFound(); 
            }

            return View(singleRecord);
        }
    }
}