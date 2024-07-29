using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherForecasterAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIconToWeatherForecast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "WeatherForecasts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "WeatherForecasts");
        }
    }
}
