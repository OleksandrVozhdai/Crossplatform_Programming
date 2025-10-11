using System.ComponentModel.DataAnnotations;

namespace disease_outbreaks_detector.Models
{
    public class CaseRecord
    {
        public int Id { get; set; }
        public string Country { get; set; } = String.Empty;
        public int Cases { get; set; }
        public int TodayCases { get; set; }
        public int Deaths { get; set; }
        public int TodayDeaths { get; set; }
        public int Recovered { get; set; }
        public int TodayRecovered { get; set; }
        public int population { get; set; }
        public int Active { get; set; } // Added from API
        public int Critical { get; set; } // Added from API
        public double? Latitude { get; set; } // Added from countryInfo
        public double? Longitude { get; set; } // Added from countryInfo
        public DateTime UpdatedAt { get; internal set; }

        // Validation method for tests
        public bool IsValid()
        {
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(this);
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            return Validator.TryValidateObject(this, context, results, true);
        }
    }
}