using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v14_Add_ChoosenPropToResponseRequestMulticastEntitiy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Choosen",
                table: "ResponseMulticastRequest",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Choosen",
                table: "ResponseMulticastRequest");
        }
    }
}
