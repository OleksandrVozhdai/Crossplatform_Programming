using Xunit;
using Moq;
using Moq.Protected;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using disease_outbreaks_detector.Controllers;
using disease_outbreaks_detector.Models;
using disease_outbreaks_detector.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text;
using System;

namespace disease_outbreaks_detector.Tests
{
    public class CaseRecordControllerTests
    {
        private AppDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
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
            var context = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var controller = new CaseRecordController(context);

            var result = await controller.GetCaseRecords();

            var okResult = Assert.IsType<ActionResult<IEnumerable<CaseRecord>>>(result);
            var value = Assert.IsAssignableFrom<List<CaseRecord>>(okResult.Value);
            Assert.Equal(2, value.Count);
        }

        [Fact]
        public async Task FetchAndStoreAsync_ReturnsValidRecord()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new AppDbContext(options);

            // Mock HttpMessageHandler
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var expectedJson = @"{
                ""country"": ""USA"",
                ""cases"": 111820082,
                ""deaths"": 1219487,
                ""recovered"": 109814428,
                ""active"": 786167,
                ""critical"": 940,
                ""population"": 334805269,
                ""updated"": 1759791280154,
                ""countryInfo"": { ""lat"": 38, ""long"": -97 }
            }";
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(expectedJson) };
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHandler.Object);
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => ((IHttpClientFactory)f).CreateClient())  // Cast to avoid extension method issue
                .Returns(httpClient);

            var service = new ExternalApi(context, mockFactory.Object);

            // Act
            var result = await service.FetchAndStoreAsync("usa");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("USA", result.Country);
            Assert.Equal(111820082, result.Cases);
        }

        [Fact]
        public async Task FetchAndStoreAsync_UpdatesExistingRecord()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new AppDbContext(options);
            context.CaseRecords.Add(new CaseRecord { Country = "usa", Cases = 100000 });
            context.SaveChanges();

            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var expectedJson = @"{
                ""country"": ""USA"",
                ""cases"": 111820082,
                ""deaths"": 1219487,
                ""recovered"": 109814428,
                ""active"": 786167,
                ""critical"": 940,
                ""population"": 334805269,
                ""updated"": 1759791280154,
                ""countryInfo"": { ""lat"": 38, ""long"": -97 }
            }";
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(expectedJson) };
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHandler.Object);
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => ((IHttpClientFactory)f).CreateClient())  // Cast to avoid extension method
                .Returns(httpClient);

            var service = new ExternalApi(context, mockFactory.Object);

            // Act
            var result = await service.FetchAndStoreAsync("usa");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(111820082, result.Cases);  // Updated
        }

        [Fact]
        public void CaseRecord_IsValid_WithFullData()
        {
            // Arrange
            var record = new CaseRecord
            {
                Country = "USA",
                Cases = 111820082,
                Deaths = 1219487
            };

            // Act & Assert
            Assert.True(record.IsValid());
        }
    }
}