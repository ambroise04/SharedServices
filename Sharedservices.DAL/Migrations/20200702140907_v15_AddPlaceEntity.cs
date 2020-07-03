using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v15_AddPlaceEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "RequestMulticasts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostalCode = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PlaceId",
                table: "Requests",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMulticasts_PlaceId",
                table: "RequestMulticasts",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestMulticasts_Places_PlaceId",
                table: "RequestMulticasts",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Places_PlaceId",
                table: "Requests",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestMulticasts_Places_PlaceId",
                table: "RequestMulticasts");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Places_PlaceId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Requests_PlaceId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_RequestMulticasts_PlaceId",
                table: "RequestMulticasts");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "RequestMulticasts");
        }
    }
}
