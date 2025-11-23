using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;

namespace disease_outbreaks_detector.Controllers
{
	public class CentralController : Controller
	{

		private readonly ApplicationDbContext _context;
		public CentralController(ApplicationDbContext context) => _context = context;

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var data = await _context.CaseRecords
				.Include(c => c.CountryEntity)
				.Include(c => c.Source)
				.ToListAsync();

				var userData = await _context.Users
			.AsNoTracking()
			.ToListAsync();

			var model = new CentralTableViewModel
			{
				CaseRecords = data,
				Users = userData
			};

			return View(model);
		}
        public async Task<IActionResult> Seed()
        {
            if (await _context.CaseRecords.CountAsync() > 10000)
            {
                return Content("База даних вже заповнена (більше 10 000 записів).");
            }

            var countries = new[] { "Ukraine", "Poland", "USA", "Germany", "France", "China", "Italy", "Spain" };
            var random = new Random();
            var recordsToAdd = new List<CaseRecord>();

            for (int i = 0; i < 11000; i++)
            {
                recordsToAdd.Add(new CaseRecord
                {
                    Country = countries[random.Next(countries.Length)],
                    Cases = random.Next(1000, 1000000),
                    TodayCases = random.Next(0, 5000),
                    Deaths = random.Next(0, 50000),
                    TodayDeaths = random.Next(0, 100),
                    Recovered = random.Next(500, 900000),
                    TodayRecovered = random.Next(0, 4000),
                    population = random.Next(1000000, 300000000),
                    Active = random.Next(100, 50000),
                    Critical = random.Next(0, 1000),
                    UpdatedAt = DateTime.Now.AddDays(-random.Next(0, 365))
                });
            }

            await _context.CaseRecords.AddRangeAsync(recordsToAdd);
            await _context.SaveChangesAsync();

            return Content($"Успішно додано {recordsToAdd.Count} записів! Тепер у базі достатньо даних.");
        }
    }
}
