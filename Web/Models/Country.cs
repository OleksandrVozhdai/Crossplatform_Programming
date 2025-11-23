using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace disease_outbreaks_detector.Models
{
	public class Country
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = null!;

		public string? IsoCode { get; set; }



		public ICollection<CaseRecord>? CaseRecords { get; set; }
	}
}
