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
                    { new Guid("9b16a86e-4412-4dcd-ad66-8435edf09c3a"), new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(1578), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(1763) },
                    { new Guid("10e0c946-9af3-49a9-95e1-01c64ca5e7cf"), new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2447), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2449) },
                    { new Guid("468ecba8-90ed-4abc-a508-638bc6a7eceb"), new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2452), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2453) },
                    { new Guid("87397d96-a5cd-4947-aeca-ef4ca2ec51ff"), new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2456), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2456) },
                    { new Guid("7c8f00d6-2c18-4756-b194-c7c52a2ab59e"), new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2459), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2460) },
                    { new Guid("bb526ff7-5615-4277-b68a-094b875414f8"), new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2462), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2463) },
                    { new Guid("533114e4-61e8-4b32-8f7e-da8fd69b3197"), new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2465), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 27, 21, 47, 19, 703, DateTimeKind.Utc).AddTicks(2466) }
                });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(6460), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(6630), "Askaiser" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7353), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7355), "Falgar" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7357), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7358), "Xenure" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7359), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7360), "Minty" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7361), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7362), "Kalmera" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7363), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7364), "Pandarf" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7365), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 27, 21, 47, 19, 701, DateTimeKind.Utc).AddTicks(7366), "Mire" }
                });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(7610), "", "", 1, new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(7789), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8555), "", "", 1, new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8560), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8562), "", "", 1, new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8563), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8564), "", "", 1, new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8565), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8566), "", "", 1, new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8567), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8568), "", "", 1, new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8569), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8571), "", "", 1, new DateTime(2021, 2, 27, 21, 47, 19, 702, DateTimeKind.Utc).AddTicks(8571), new Guid("00000000-0000-0000-0000-000000000007") }
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
