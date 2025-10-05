using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using disease_outbreaks_detector.Controllers;
using disease_outbreaks_detector.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace disease_outbreaks_detector.Tests
{
	public class CaseRecordControllerTests
	{
		private AppDbContext GetInMemoryDbContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDb")
				.Options;

			var context = new AppDbContext(options);

			context.CaseRecords.AddRange(new List<CaseRecord>
			{
				new CaseRecord { Id = 1, Country = "USA", Cases = 100 },
				new CaseRecord { Id = 2, Country = "UK", Cases = 50 }
			});
			context.SaveChanges();

			return context;
		}

		[Fact]
		public async Task GetCaseRecords_Returns200Ok()
		{
			var context = GetInMemoryDbContext();
			var controller = new CaseRecordController(context);

			var result = await controller.GetCaseRecords();

			var okResult = Assert.IsType<ActionResult<IEnumerable<CaseRecord>>>(result);
			var value = Assert.IsAssignableFrom<List<CaseRecord>>(okResult.Value);
			Assert.Equal(2, value.Count); 
		}
	}
}
