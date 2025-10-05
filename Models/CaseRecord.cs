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
		public DateTime UpdatedAt { get; internal set; }
	}
}
