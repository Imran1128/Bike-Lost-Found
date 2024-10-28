using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeLostAndFound.Migrations
{
    public partial class ad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bikeAdvertisements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: false),
                    Model = table.Column<string>(nullable: true),
                    YearOfManufacture = table.Column<string>(nullable: false),
                    EngineCapacity = table.Column<string>(nullable: false),
                    KilometersRun = table.Column<string>(nullable: false),
                    OwnerName = table.Column<string>(nullable: true),
                    OwnerEmail = table.Column<string>(nullable: false),
                    OwnerPhone = table.Column<string>(nullable: true),
                    OwnerAddress = table.Column<string>(nullable: true),
                    BikePhoto = table.Column<string>(nullable: true),
                    BikeRegNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bikeAdvertisements", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bikeAdvertisements");
        }
    }
}
