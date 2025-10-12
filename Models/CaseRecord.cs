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
        public int Active { get; set; } // Added from API
        public int Critical { get; set; } // Added from API
        public double? Latitude { get; set; } // Added from countryInfo
        public double? Longitude { get; set; } // Added from countryInfo

        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime UpdatedAt { get; set; }

        // Validation method for tests
        public bool IsValid()
        {
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(this);
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            return Validator.TryValidateObject(this, context, results, true);
        }
    }

    // Custom JsonConverter for Unix timestamp (milliseconds) to DateTime
    public class UnixTimestampConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                long timestamp = reader.GetInt64();
                return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
            }
            return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeMilliseconds());
        }
    }
}