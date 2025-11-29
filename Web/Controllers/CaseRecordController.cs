using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Data;

namespace disease_outbreaks_detector.Controllers
{
	// Підтримуємо версії 1.0 і 2.0
	[ApiVersion("1.0")]
	[ApiVersion("2.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	public class CaseRecordController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public CaseRecordController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/v{version}/CaseRecord
		// Доступний у обох версіях (v1 і v2) — сумісність з попередньою версією
		[HttpGet]
		[MapToApiVersion("1.0")]
		[MapToApiVersion("2.0")]
		public async Task<ActionResult<IEnumerable<CaseRecord>>> GetCaseRecords()
		{
			return await _context.CaseRecords.ToListAsync();
		}

		// GET: api/v{version}/CaseRecord/Name/{country}
		// Доступний у обох версіях — знайти запис по назві
		[HttpGet("Name/{country}")]
		[MapToApiVersion("2.0")]
		public async Task<ActionResult<CaseRecord>> GetCaseRecordByCountry(string country)
		{
			var record = await _context.CaseRecords
				.FirstOrDefaultAsync(c => c.Country.ToLower() == country.ToLower());

			if (record == null)
			{
				return NotFound($"No case record found for country: {country}");
			}

			return record;
		}

		// GET: api/v{version}/CaseRecord/{id}
		[HttpGet("{id}")]
		[MapToApiVersion("1.0")]
		[MapToApiVersion("2.0")]
		public async Task<ActionResult<CaseRecord>> GetCaseRecord(int id)
		{
			var caseRecord = await _context.CaseRecords.FindAsync(id);

			if (caseRecord == null)
			{
				return NotFound();
			}

			return caseRecord;
		}

		// NEW in v2: GET list of country names
		// GET: api/v2/CaseRecord/countries
		// ЦЕЙ ендпоінт є ТІЛЬКИ в версії 2.0
		[HttpGet("countries")]
		[MapToApiVersion("2.0")]
		public async Task<ActionResult<IEnumerable<string>>> GetCountryNames()
		{
			var names = await _context.CaseRecords
				.Select(c => c.Country)
				.Where(n => !string.IsNullOrEmpty(n))
				.Distinct()
				.ToListAsync();

			return names;
		}

		// PUT, POST, DELETE — залишаємо як було; мапимо на обидві версії
		[HttpPut("{id}")]
		[MapToApiVersion("1.0")]
		[MapToApiVersion("2.0")]
		public async Task<IActionResult> PutCaseRecord(int id, CaseRecord caseRecord)
		{
			if (id != caseRecord.Id) return BadRequest();

			_context.Entry(caseRecord).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CaseRecordExists(id)) return NotFound();
				throw;
			}
			return NoContent();
		}

		[HttpPost]
		[MapToApiVersion("1.0")]
		[MapToApiVersion("2.0")]
		public async Task<ActionResult<CaseRecord>> PostCaseRecord(CaseRecord caseRecord)
		{
			var country = await _context.Countries
				.FirstOrDefaultAsync(c => c.Name == caseRecord.Country);

			if (country == null)
			{
				country = new Country { Name = caseRecord.Country };
				_context.Countries.Add(country);
				await _context.SaveChangesAsync();
			}

			caseRecord.CountryId = country.Id;

			_context.CaseRecords.Add(caseRecord);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetCaseRecord), new { id = caseRecord.Id, version = "2.0" }, caseRecord);
		}

		[HttpDelete("{id}")]
		[MapToApiVersion("1.0")]
		[MapToApiVersion("2.0")]
		public async Task<IActionResult> DeleteCaseRecord(int id)
		{
			var caseRecord = await _context.CaseRecords.FindAsync(id);
			if (caseRecord == null) return NotFound();

			_context.CaseRecords.Remove(caseRecord);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpGet("login")]
		[MapToApiVersion("1.0")]
		[MapToApiVersion("2.0")]
		public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return BadRequest("Email is required.");

			var user = await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
				return NotFound("User not found.");

			return Ok(user);
		}



		private bool CaseRecordExists(int id) =>
			_context.CaseRecords.Any(e => e.Id == id);
	}
}
