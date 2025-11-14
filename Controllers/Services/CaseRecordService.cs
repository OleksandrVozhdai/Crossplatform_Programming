using disease_outbreaks_detector.Data;
using disease_outbreaks_detector.Models;
using Microsoft.EntityFrameworkCore;

public class CaseRecordService
{
	private readonly ApplicationDbContext _context;

	public CaseRecordService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task AddCaseRecordAsync(CaseRecord record)
	{
	
		var country = await _context.Countries
			.FirstOrDefaultAsync(c => c.Name == record.Country);

		if (country == null)
		{
			// Якщо немає – додаємо її
			country = new Country { Name = record.Country };
			_context.Countries.Add(country);
			await _context.SaveChangesAsync(); 
		}


		record.CountryId = country.Id;


		_context.CaseRecords.Add(record);
		await _context.SaveChangesAsync();
	}
}
