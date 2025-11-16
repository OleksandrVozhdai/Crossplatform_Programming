using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Важливо додати це

namespace disease_outbreaks_detector.Controllers
{
    public class CentralController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CentralController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allCases = await _context.CaseRecords.ToListAsync();
            return View(allCases);
        }
        public async Task<IActionResult> Details(int id)
        {
            var singleRecord = await _context.CaseRecords.FirstOrDefaultAsync(record => record.Id == id);

            if (singleRecord == null)
            {
                return NotFound();
            }
            return View(singleRecord);
        }
    }
}