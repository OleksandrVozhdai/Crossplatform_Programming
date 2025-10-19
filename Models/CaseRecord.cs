using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace disease_outbreaks_detector.Models
{
    public class CaseRecord
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        public int Cases { get; set; }
        public int TodayCases { get; set; }
        public int Deaths { get; set; }
        public int TodayDeaths { get; set; }
        public int Recovered { get; set; }
        public int TodayRecovered { get; set; }
        public int population { get; set; }
        public int Active { get; set; } 
        public int Critical { get; set; } 
        public double? Latitude { get; set; } 
        public double? Longitude { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Today.AddDays(-1);

		// Validation method for tests
		public bool IsValid()
        {
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(this);
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            return Validator.TryValidateObject(this, context, results, true);
        }
    }

   
}