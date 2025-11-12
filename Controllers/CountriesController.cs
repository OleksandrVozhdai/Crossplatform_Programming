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
