using Microsoft.AspNetCore.Mvc;

namespace disease_outbreaks_detector.Models
{
	public class CentralTableViewModel 
	{
		public List<CaseRecord> CaseRecords { get; set; }
		public List<AppDbContextUser> Users { get; set; }
	}
}
