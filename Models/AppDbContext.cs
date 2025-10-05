using Microsoft.EntityFrameworkCore;

namespace disease_outbreaks_detector.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<CaseRecord> CaseRecords { get; set; }
	}
}
