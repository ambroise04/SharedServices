using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v2_AddDefaultPointForUserColumnToGlobalInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultPointForUsers",
                table: "Infos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Infos",
                keyColumn: "Id",
                keyValue: 1,
                column: "DefaultPointForUsers",
                value: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultPointForUsers",
                table: "Infos");
        }
    }
}
