using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ODBP.Migrations
{
    /// <inheritdoc />
    public partial class AddWeatherForecastTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TemperatureC = table.Column<int>(type: "integer", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Guid);
                });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Guid", "Date", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { new Guid("4fa89832-2627-412e-b749-4b4da3ee0f93"), new DateOnly(2024, 8, 31), "Warm", 25 },
                    { new Guid("535309b4-d6ef-42eb-8306-9ab834b795d8"), new DateOnly(2024, 9, 3), "Chilly", 10 },
                    { new Guid("66a7c17a-2822-4dd1-b9cd-b18d6ebc4bb9"), new DateOnly(2024, 9, 1), "Cool", 15 },
                    { new Guid("cc957f44-67fb-43ee-9eb9-8a263fd80375"), new DateOnly(2024, 9, 2), "Hot", 30 },
                    { new Guid("e31771d6-c6c4-4ff8-a2da-72b90847ac27"), new DateOnly(2024, 8, 30), "Mild", 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }
    }
}
