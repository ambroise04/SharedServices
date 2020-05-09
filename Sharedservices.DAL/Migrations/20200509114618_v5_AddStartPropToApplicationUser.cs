using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v5_AddStartPropToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Start",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Start",
                table: "AspNetUsers");
        }
    }
}
