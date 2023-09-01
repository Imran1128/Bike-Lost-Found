using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeLostAndFound.Migrations
{
    public partial class UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LostAndFoundBikeInformation",
                table: "LostAndFoundBikeInformation");

            migrationBuilder.RenameTable(
                name: "LostAndFoundBikeInformation",
                newName: "lostAndFoundBikeInformation");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "lostAndFoundBikeInformation",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_lostAndFoundBikeInformation",
                table: "lostAndFoundBikeInformation",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_lostAndFoundBikeInformation",
                table: "lostAndFoundBikeInformation");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "lostAndFoundBikeInformation");

            migrationBuilder.RenameTable(
                name: "lostAndFoundBikeInformation",
                newName: "LostAndFoundBikeInformation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LostAndFoundBikeInformation",
                table: "LostAndFoundBikeInformation",
                column: "Id");
        }
    }
}
