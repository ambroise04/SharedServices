using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v19_AddDurationAndDescriptionFieldsToRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RequestMulticasts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "RequestMulticasts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RequestMulticasts");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "RequestMulticasts");
        }
    }
}
