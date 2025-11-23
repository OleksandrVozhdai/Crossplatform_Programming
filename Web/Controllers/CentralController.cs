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

	}
}
