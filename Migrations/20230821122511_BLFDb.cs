using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeLostAndFound.Migrations
{
    public partial class BLFDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LostAndFoundBikeInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BikeName = table.Column<string>(nullable: true),
                    BikeRegNo = table.Column<string>(nullable: true),
                    BikeSN = table.Column<string>(nullable: true),
                    PlaceWhereLost = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    OwnerName = table.Column<string>(nullable: true),
                    BikePhoto = table.Column<string>(nullable: true),
                    FoundOrLost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostAndFoundBikeInformation", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LostAndFoundBikeInformation");
        }
    }
}
