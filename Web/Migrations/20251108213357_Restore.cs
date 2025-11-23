using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace disease_outbreaks_detector.Migrations
{
    /// <inheritdoc />
    public partial class Restore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecords_Countries_CountryId",
                table: "CaseRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecords_Sources_SourceId",
                table: "CaseRecords");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "CaseRecords",
                newName: "Country");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecords_Countries_CountryId",
                table: "CaseRecords",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecords_Sources_SourceId",
                table: "CaseRecords",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecords_Countries_CountryId",
                table: "CaseRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseRecords_Sources_SourceId",
                table: "CaseRecords");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "CaseRecords",
                newName: "CountryName");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecords_Countries_CountryId",
                table: "CaseRecords",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRecords_Sources_SourceId",
                table: "CaseRecords",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
