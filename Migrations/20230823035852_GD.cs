using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeLostAndFound.Migrations
{
    public partial class GD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerEmail",
                table: "LostAndFoundBikeInformation",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GDNumber",
                table: "LostAndFoundBikeInformation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GDNumber",
                table: "LostAndFoundBikeInformation");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerEmail",
                table: "LostAndFoundBikeInformation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
