# Disease Outbreaks Detector

## Overview
This is a cross-platform C# application designed for monitoring COVID-19 outbreaks. It fetches real-time data from the Disease.sh API, stores it in a local SQLite database using Entity Framework Core, and provides a RESTful API for CRUD operations. The application includes unit tests for reliability, CI/CD via GitHub Actions, and a basic Razor Pages frontend for data visualization. The focus is on simplicity and extensibility, following lab requirements for cross-platform programming.

The app seeds initial data for "USA" on startup and supports fetching data for any country via the API. It's built with .NET 8 and ASP.NET Core, making it runnable on Windows, macOS, or Linux.

## Technologies
- **Backend**: .NET 8, ASP.NET Core Web API
- **Database**: SQLite (local file-based, via Entity Framework Core 9.0.9)
- **External API**: Disease.sh COVID-19 API (GET /v3/covid-19/countries/{country})
- **ORM**: Entity Framework Core (with migrations)
- **Testing**: xUnit, Moq (for mocks)
- **CI/CD**: GitHub Actions (build and test on push/PR)
- **Frontend**: Razor Pages (MVC-style, pure HTML/CSS, no JS frameworks)
- **Documentation**: Swagger UI for API exploration
- **Dependencies**: Swashbuckle.AspNetCore (Swagger), Microsoft.EntityFrameworkCore.Sqlite, Moq, xUnit

## Prerequisites
- .NET 8 SDK (download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0))
- Git (for cloning)
- Visual Studio 2022 or VS Code with C# extension (optional, for development)
- SQLite tools (optional, for DB inspection)

## Setup and Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/OleksandrVozhdai/Crossplatform_Programming.git
    cd Crossplatform_Programming
    ```

2. **Restore NuGet packages**:
    ```bash
    dotnet restore
    ```
    This installs all dependencies (EF Core, Moq, Swagger, etc.).

3. **Apply database migrations**:
    - Open the project in Visual Studio or use PMC (Package Manager Console).
    - Run:
      ```bash
      dotnet ef migrations add InitialCreate
      dotnet ef database update
      ```
    This creates the `diseaseOutbreaksDB.db` file in the project root and the `CaseRecords` table.

4. **Run the application**:
    ```bash
    dotnet run --launch-profile "http"
    ```
    The app starts on `http://localhost:5052` (HTTP profile for simplicity). On startup, it seeds data for "USA" from the Disease.sh API.

5. **Access the app**:
    - **Swagger UI** (API docs): [http://localhost:5052/swagger](http://localhost:5052/swagger)
    - **Frontend**: [http://localhost:5052/Cases/Index](http://localhost:5052/Cases/Index) (basic Razor page for data display)

6. **Run tests**:
    ```bash
    dotnet test
    ```
    All 4 unit tests should pass (coverage for controller, ExternalApi fetch/update, model validation).

## API Endpoints

The API is self-documented in Swagger. Base URL: `http://localhost:5052/api/caserecord`.

| Method | Endpoint | Description | Request Body | Response Example |
|--------|----------|-------------|--------------|------------------|
| GET    | /api/caserecord | Get all case records from DB | N/A | `[{"id":1,"country":"USA","cases":111820082,"deaths":1219487,"recovered":109814428,"population":334805269,"updatedAt":"2025-10-11T00:00:00Z"}]` |
| GET    | /api/caserecord/{id} | Get record by ID | N/A | `{"id":1,"country":"USA","cases":111820082,...}` |
| POST   | /api/caserecord | Create new record | CaseRecord JSON | `201 Created` with new record |
| PUT    | /api/caserecord/{id} | Update record by ID | CaseRecord JSON | `204 No Content` |
| DELETE | /api/caserecord/{id} | Delete record by ID | N/A | `204 No Content` |

### Example API Usage (Postman/Swagger)
- **Fetch and store data**: The app automatically fetches on startup (USA). To fetch for another country, use the frontend or extend the API with POST `/api/caserecord/{country}` (future enhancement).
- **Error handling**: 404 for not found, 400 for invalid data (validation on Country).

## External API Integration

The app integrates with [Disease.sh COVID-19 API](https://disease.sh/docs/#/COVID-19%3A%20Worldometers/get_v3_covid_19_countries__country_):
- Endpoint: `https://disease.sh/v3/covid-19/countries/{country}?yesterday=true&strict=true`
- Response example (USA):
  ```json
  {
    "country": "USA",
    "cases": 111820082,
    "deaths": 1219487,
    "recovered": 109814428,
    "active": 786167,
    "critical": 940,
    "population": 334805269,
    "updated": 1759791280154,
    "countryInfo": {
      "lat": 38,
      "long": -97
    }
  }

  Mapping: Deserialized to `CaseRecord` model using `System.Text.Json` (case-insensitive).  
Persistence: Stored in SQLite if not exists; updated if exists. Geo data (Latitude/Longitude) parsed from "countryInfo".

### Models
The core model is `CaseRecord`, representing outbreak data per country.

```csharp
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
    public int Population { get; set; }
    public int Active { get; set; }
    public int Critical { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime UpdatedAt { get; set; }

    // Validation for tests
    public bool IsValid()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();
        return Validator.TryValidateObject(this, context, results, true);
    }
}

## UnixTimestampConverter

A custom JSON converter is used to handle the updated timestamp in the API response. It converts the timestamp from Unix format (milliseconds) to `DateTime`:

```csharp
public class UnixTimestampConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).DateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeMilliseconds());
    }
}

## AppDbContext

`AppDbContext` is used as the EF Core DbContext with a `DbSet<CaseRecord>` to interact with the `CaseRecords` table in SQLite.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<CaseRecord> CaseRecords { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaseRecord>()
            .HasIndex(c => c.Country)
            .IsUnique();
    }
}

## Tests

Unit tests use xUnit and Moq for mocking. Coverage: 80% for services and controllers.

### Run tests:
```bash
dotnet test

## Test Coverage

- **GetCaseRecords_Returns200Ok**: Verifies that the controller correctly returns a list from the database.
- **FetchAndStoreAsync_ReturnsValidRecord**: Mocks an API response, verifies that data is correctly mapped and stored.
- **FetchAndStoreAsync_UpdatesExistingRecord**: Tests the update behavior for an existing country record.
- **CaseRecord_IsValid_WithFullData**: Validates the model with sample data to ensure correct validation rules.

## Frontend

A basic Razor Page at `/Cases/Index` is provided for data visualization. Features include:

- A search form to retrieve data for a specific country.
- Stats cards to display key metrics (Cases, Deaths, Recovered, Population, Updated).
- Pure HTML/CSS, responsive grid layout (no JS frameworks).

Example: Navigate to [http://localhost:5052/Cases/Index?country=Ukraine](http://localhost:5052/Cases/Index) — displays data for Ukraine.

### Code snippet (Views/Cases/Index.cshtml):

```html
<div class="stats-grid">
    <div class="stat-card">
        <h3>Total Cases</h3>
        <p>@record.Cases.ToString("N0")</p>
    </div>
    <!-- More cards for Deaths, Recovered, etc. -->
</div>

<form asp-action="Index" method="get">
    <input type="text" name="country" value="@country" placeholder="e.g., Ukraine" />
    <button type="submit">Fetch Data</button>
</form>

<style>
    .stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 15px; }
    .stat-card { border: 1px solid #ddd; padding: 15px; text-align: center; background: #f8f9fa; }
</style>

## CI/CD

The GitHub Actions workflow file `.github/workflows/dotnet.yml` is set up for continuous integration and deployment. It triggers on push or pull requests to the `main` branch, and it includes the following steps:

- **Restore**: Install dependencies.
- **Build**: Compile the project.
- **Test**: Run unit tests on Ubuntu with .NET 8.

The build status is visible in the GitHub Actions tab for the repository.

## Contribution Guidelines

- **Branching**: Use `feature/lab1-[yourname]` for your changes.
- **Commits**: Write descriptive commit messages (e.g., "Add unit tests for ExternalApi").
- **Tests**: Run `dotnet test` before pushing; aim for 80% code coverage.
- **Pull Requests**: Include a description, test results, and any next steps for the project.

## License

MIT License — see LICENSE file for details.
