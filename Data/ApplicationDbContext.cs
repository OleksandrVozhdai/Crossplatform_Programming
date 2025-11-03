using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using disease_outbreaks_detector.Models;

namespace disease_outbreaks_detector.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppDbContextUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

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