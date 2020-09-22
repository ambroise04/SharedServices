using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedServices.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PostalCode = table.Column<int>(nullable: false),
                    Point = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Start = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Infos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(nullable: false),
                    DescriptionFR = table.Column<string>(nullable: false),
                    DescriptionEN = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    AddressFR = table.Column<string>(nullable: false),
                    AddressEN = table.Column<string>(nullable: true),
                    AuthorLink = table.Column<string>(nullable: false),
                    DefaultPointForUsers = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(nullable: false),
                    MessageFR = table.Column<string>(nullable: true),
                    MessageEN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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

            migrationBuilder.CreateTable(
                name: "ServiceGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: false),
                    PointsByHour = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ip = table.Column<string>(nullable: true),
                    Hostname = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Loc = table.Column<string>(nullable: true),
                    Org = table.Column<string>(nullable: true),
                    Postal = table.Column<string>(nullable: true),
                    SessionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discussions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Emitter = table.Column<string>(nullable: false),
                    Receiver = table.Column<string>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    DateHour = table.Column<DateTime>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discussions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FaqQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqQuestions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mark = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Advisor = table.Column<string>(nullable: false),
                    DisplayAdvisorName = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContentType = table.Column<string>(nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_ServiceGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ServiceGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaqResponses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    QuestionId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqResponses_FaqQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "FaqQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserServices",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserServices", x => new { x.ServiceId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserServices_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestMulticasts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RequesterMulticastId = table.Column<string>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    DateOfRequest = table.Column<DateTime>(nullable: false),
                    DateOfAddition = table.Column<DateTime>(nullable: false),
                    Point = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Accepted = table.Column<bool>(nullable: false),
                    PlaceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestMulticasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestMulticasts_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReceiverId = table.Column<string>(nullable: true),
                    RequesterId = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    DateOfRequest = table.Column<DateTime>(nullable: false),
                    DateOfAddition = table.Column<DateTime>(nullable: false),
                    Point = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    PlaceId = table.Column<int>(nullable: true),
                    RequesterValidation = table.Column<bool>(nullable: false),
                    ReceiverValidation = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_AspNetUsers_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Services_ServiceId",
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
                    ApplicationUserId = table.Column<string>(nullable: false),
                    Choosen = table.Column<bool>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                table: "Infos",
                columns: new[] { "Id", "AddressEN", "AddressFR", "AuthorLink", "DefaultPointForUsers", "DescriptionEN", "DescriptionFR", "Email", "Phone" },
                values: new object[] { 1, "Place Cardinal Mercier, 2 Wavre Belgium", "Place Cardinal Mercier, 2 Wavre Belgique", "https://www.labak.azurewebsites.net", 10, "Description of this platform", "Description de cette plateforme", "labakoam@gmail.com", "+32 (0)494 68 00 38" });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "MessageEN", "MessageFR", "Type" },
                values: new object[] { 4, "A new request has been published. Do you want to know more ?", "Une nouvelle demande a été publiée. Voulez-vous en savoir plus ?", 3 });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "MessageEN", "MessageFR", "Type" },
                values: new object[] { 1, "Your correspondent has marked a service as rendered. Your confirmation is required for the transfer of points.", "Votre correspondant a marqué un service comme rendu. Veuillez donner votre accord pour le transfert des points.", 0 });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "MessageEN", "MessageFR", "Type" },
                values: new object[] { 2, "Your correspondent has marked a service as rendered. Please accept the transfer of points.", "Votre correspondant a marqué un service comme rendu. Veuillez accepter le transfert des points", 1 });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "MessageEN", "MessageFR", "Type" },
                values: new object[] { 3, "New response to your request. Consult the list of requests ?", "Une nouvelle réponse a votre demande a été envoyée. Consulter la liste des demandes ?", 2 });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 1, 15, "Déménagement" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 2, 15, "Jardinage" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 3, 15, "Bricolage" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 4, 15, "Babysitting" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 12, 15, "Courses et Démarches" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 6, 15, "Cours particuliers" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 7, 15, "Tâches ménagères" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 8, 15, "Informatique" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 9, 15, "Soins et Beauté" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 10, 15, "Evénements" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 11, 15, "Entretien" });

            migrationBuilder.InsertData(
                table: "ServiceGroups",
                columns: new[] { "Id", "PointsByHour", "Title" },
                values: new object[] { 5, 15, "Animaux" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 2, "Vous aider à déplacer vos meubles et autres.", 1, "Aide déménagement" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 7, "Garder les enfants, les amuser le temps que les parents reviennent", 2, "Tondre une pelouse" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 9, "Rendre la clôture de vos maisons belle et attirante.", 2, "Tondre une haie" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 3, "Habitué des meubles, je peux vous aider à monter les vôtres.", 3, "Montage de meubles en kit" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 5, "Je peux vous aider à faire les finitions", 3, "Maçonnerie" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 6, "Aide à la décoration pour vos événements.", 3, "Décoration" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 1, "Garder les enfants, les amuser le temps que les parents reviennent.", 4, "Garde d'enfant" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 8, "Garder les grands-parents, les aider et leur faire rire.", 4, "Garde de personnes agées" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 4, "Fan des animaux, j'adore les garder et les promener", 5, "Garde d'animaux" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 10, "Vous aider dans vos différentes tâches ménagères.", 7, "Aide ménagère" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "GroupId", "Title" },
                values: new object[] { 11, "Repasser vos linges.", 7, "Repassage" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserServices_ApplicationUserId",
                table: "ApplicationUserServices",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicationUserId",
                table: "AspNetUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_ApplicationUserId",
                table: "Discussions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqQuestions_UserId",
                table: "FaqQuestions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqResponses_QuestionId",
                table: "FaqResponses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ApplicationUserId",
                table: "Pictures",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestMulticasts_PlaceId",
                table: "RequestMulticasts",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMulticasts_RequesterMulticastId",
                table: "RequestMulticasts",
                column: "RequesterMulticastId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestMulticasts_ServiceId",
                table: "RequestMulticasts",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PlaceId",
                table: "Requests",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ReceiverId",
                table: "Requests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequesterId",
                table: "Requests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ServiceId",
                table: "Requests",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseMulticastRequest_ApplicationUserId",
                table: "ResponseMulticastRequest",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_GroupId",
                table: "Services",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserServices");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Discussions");

            migrationBuilder.DropTable(
                name: "FaqResponses");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Infos");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "ResponseMulticastRequest");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "FaqQuestions");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "RequestMulticasts");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "ServiceGroups");
        }
    }
}
