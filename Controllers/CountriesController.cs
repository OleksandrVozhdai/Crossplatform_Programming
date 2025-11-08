using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace disease_outbreaks_detector.Controllers
{
	public class CountriesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CountriesController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			
			var countries = await _context.CaseRecords
				.Select(c => new CountryViewModel
				{
					Id = c.Id,
					Name = c.Country
				})
				.ToListAsync();

			return View(countries);
		}

		public async Task<IActionResult> Details(int id)
		{
			var country = await _context.CaseRecords
				.FirstOrDefaultAsync(c => c.Id == id);

			if (country == null)
				return NotFound();

			return View(country);
		}
	}
}
