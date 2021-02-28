using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bomberjam.Website.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "App_Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitDuration = table.Column<double>(type: "float", nullable: true),
                    GameDuration = table.Column<double>(type: "float", nullable: true),
                    Stdout = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stderr = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GithubId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Points = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Bots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Bots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_Bots_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "App_GameUsers",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeltaPoints = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_GameUsers", x => new { x.GameId, x.UserId });
                    table.ForeignKey(
                        name: "FK_App_GameUsers_App_Bots_BotId",
                        column: x => x.BotId,
                        principalTable: "App_Bots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_App_GameUsers_App_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "App_Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_App_GameUsers_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[,]
                {
                    { new Guid("0adc087c-f2e6-428a-9381-a4fa3b2c000d"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(7627), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(7802) },
                    { new Guid("63b0e61b-9062-4b72-a094-b9a795272d9d"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8484), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8486) },
                    { new Guid("bd1d2b4b-5c9b-4cfb-a420-23a9c8098b65"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8490), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8491) },
                    { new Guid("9ba9871d-cc45-4d4a-b452-ced7e72446e7"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8493), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8494) },
                    { new Guid("297c7377-c487-4f77-a125-772e3224b393"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8496), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8497) },
                    { new Guid("2eb88f49-f37f-47cc-ad6c-bf2996900d4f"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8513), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8513) },
                    { new Guid("db610158-ad68-46e0-bcdc-ac7ffe5d8e47"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8516), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(8517) }
                });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(2826), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3093), "Askaiser" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3682), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3684), "Falgar" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3686), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3686), "Xenure" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3688), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3689), "Minty" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3690), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3691), "Kalmera" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3693), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3694), "Pandarf" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3695), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 28, 3, 50, 16, 680, DateTimeKind.Utc).AddTicks(3696), "Mire" }
                });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(3680), "", "", 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(3862), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4639), "", "", 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4644), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4647), "", "", 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4648), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4649), "", "", 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4650), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4651), "", "", 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4652), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4653), "", "", 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4654), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4655), "", "", 1, new DateTime(2021, 2, 28, 3, 50, 16, 681, DateTimeKind.Utc).AddTicks(4656), new Guid("00000000-0000-0000-0000-000000000007") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_App_Bots_Created",
                table: "App_Bots",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_App_Bots_Status",
                table: "App_Bots",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_App_Bots_Updated",
                table: "App_Bots",
                column: "Updated");

            migrationBuilder.CreateIndex(
                name: "IX_App_Bots_UserId",
                table: "App_Bots",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Games_Created",
                table: "App_Games",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_App_GameUsers_BotId",
                table: "App_GameUsers",
                column: "BotId");

            migrationBuilder.CreateIndex(
                name: "IX_App_GameUsers_UserId",
                table: "App_GameUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Tasks_Created",
                table: "App_Tasks",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_App_Tasks_Status",
                table: "App_Tasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_App_Tasks_Type",
                table: "App_Tasks",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_Email",
                table: "App_Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_GithubId",
                table: "App_Users",
                column: "GithubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_Points",
                table: "App_Users",
                column: "Points");

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_UserName",
                table: "App_Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_GameUsers");

            migrationBuilder.DropTable(
                name: "App_Tasks");

            migrationBuilder.DropTable(
                name: "App_Bots");

            migrationBuilder.DropTable(
                name: "App_Games");

            migrationBuilder.DropTable(
                name: "App_Users");
        }
    }
}
