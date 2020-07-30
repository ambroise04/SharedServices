using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class v18_AddNotificationAndNotificationTypeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    MessageFR = table.Column<string>(nullable: true),
                    MessageEN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    CorrespondentId = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    RequestId = table.Column<int>(nullable: true),
                    RequestMulticastId = table.Column<int>(nullable: true),
                    ServiceId = table.Column<int>(nullable: true),
                    IsTriggered = table.Column<bool>(nullable: false),
                    DateOfAddition = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_CorrespondentId",
                        column: x => x.CorrespondentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_RequestMulticasts_RequestMulticastId",
                        column: x => x.RequestMulticastId,
                        principalTable: "RequestMulticasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "MessageEN", "MessageFR", "Type" },
                values: new object[,]
                {
                    { 1, "Your correspondent has marked a service as rendered. Your confirmation is required for the transfer of points.", "Votre correspondant a marqué un service comme rendu. Veuillez donner votre accord pour le transfert des points.", 0 },
                    { 2, "Your correspondent has marked a service as rendered. Please accept the transfer of points.", "Votre correspondant a marqué un service comme rendu. Veuillez accepter le transfert des points", 1 },
                    { 3, "New response to your request. Consult the list of requests ?", "Une nouvelle réponse a votre demande a été envoyée. Consulter la liste des demandes ?", 2 },
                    { 4, "A new request has been published. Do you want to know more ?", "Une nouvelle demande a été publiée. Voulez-vous en savoir plus ?", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CorrespondentId",
                table: "Notifications",
                column: "CorrespondentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RequestId",
                table: "Notifications",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RequestMulticastId",
                table: "Notifications",
                column: "RequestMulticastId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ServiceId",
                table: "Notifications",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TypeId",
                table: "Notifications",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationTypes");
        }
    }
}
