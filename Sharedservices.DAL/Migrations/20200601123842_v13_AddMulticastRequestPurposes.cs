using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v13_AddMulticastRequestPurposes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestMulticasts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterMulticastId = table.Column<string>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    DateOfRequest = table.Column<DateTime>(nullable: false),
                    DateOfAddition = table.Column<DateTime>(nullable: false),
                    Point = table.Column<int>(nullable: false),
                    Accepted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestMulticasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestMulticasts_AspNetUsers_RequesterMulticastId",
                        column: x => x.RequesterMulticastId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestMulticasts_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponseMulticastRequest",
                columns: table => new
                {
                    RequestMulticastId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseMulticastRequest", x => new { x.RequestMulticastId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_ResponseMulticastRequest_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResponseMulticastRequest_RequestMulticasts_RequestMulticastId",
                        column: x => x.RequestMulticastId,
                        principalTable: "RequestMulticasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestMulticasts_RequesterMulticastId",
                table: "RequestMulticasts",
                column: "RequesterMulticastId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMulticasts_ServiceId",
                table: "RequestMulticasts",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseMulticastRequest_ApplicationUserId",
                table: "ResponseMulticastRequest",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseMulticastRequest");

            migrationBuilder.DropTable(
                name: "RequestMulticasts");
        }
    }
}
