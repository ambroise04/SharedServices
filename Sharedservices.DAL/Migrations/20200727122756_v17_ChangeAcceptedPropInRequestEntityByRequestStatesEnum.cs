using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v17_ChangeAcceptedPropInRequestEntityByRequestStatesEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Requests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Requests");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
