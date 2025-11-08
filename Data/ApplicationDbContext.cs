using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using disease_outbreaks_detector.Models;
using System.Reflection.Emit;

namespace disease_outbreaks_detector.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppDbContextUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

		public DbSet<CaseRecord> CaseRecords { get; set; } = null!;
		public DbSet<Country> Countries { get; set; } = null!;
		public DbSet<Source> Sources { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppDbContextUser>(entity =>
            {
                entity.Property(u => u.UserName).HasMaxLength(50);
                entity.Property(u => u.FullName).HasMaxLength(500);
                entity.HasIndex(u => u.UserName).IsUnique();
			});
		}
    }
}