using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
	public class CaseRecord
	{
		public int Id { get; set; }
		public string? Country { get; set; }
		public long Cases { get; set; }
		public long TodayCases { get; set; }
		public long Deaths { get; set; }
		public long TodayDeaths { get; set; }
		public long Recovered { get; set; }
		public long TodayRecovered { get; set; }
		public long Population { get; set; }
		public long Active { get; set; }
		public long Critical { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
