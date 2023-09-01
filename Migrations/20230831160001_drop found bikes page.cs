using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeLostAndFound.Migrations
{
    public partial class dropfoundbikespage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BikeLost",
                table: "lostAndFoundBikeInformation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BikeLost",
                table: "lostAndFoundBikeInformation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
