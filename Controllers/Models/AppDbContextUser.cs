using Microsoft.AspNetCore.Identity;

namespace disease_outbreaks_detector.Models
{
    public class AppDbContextUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}