using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeLostAndFound.Migrations
{
    public partial class LostAndFound : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoundOrLost",
                table: "LostAndFoundBikeInformation");

            migrationBuilder.AddColumn<bool>(
                name: "BikeLost",
                table: "LostAndFoundBikeInformation",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BikeLost",
                table: "LostAndFoundBikeInformation");

            migrationBuilder.AddColumn<int>(
                name: "FoundOrLost",
                table: "LostAndFoundBikeInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
