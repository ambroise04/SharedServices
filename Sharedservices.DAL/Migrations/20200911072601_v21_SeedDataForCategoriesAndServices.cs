using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v21_SeedDataForCategoriesAndServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceGroups_GroupId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Services",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceGroups_GroupId",
                table: "Services",
                column: "GroupId",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceGroups_GroupId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceGroups_GroupId",
                table: "Services",
                column: "GroupId",
                principalTable: "ServiceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
