using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace disease_outbreaks_detector.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "CaseRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Critical",
                table: "CaseRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "CaseRecords",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "CaseRecords",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "CaseRecords");

            migrationBuilder.DropColumn(
                name: "Critical",
                table: "CaseRecords");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "CaseRecords");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "CaseRecords");
        }
    }
}
