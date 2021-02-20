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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Errors = table.Column<string>(type: "TEXT", nullable: true),
                    InitDuration = table.Column<double>(type: "REAL", nullable: true),
                    GameDuration = table.Column<double>(type: "REAL", nullable: true),
                    Stdout = table.Column<string>(type: "TEXT", nullable: true),
                    Stderr = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GithubId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Points = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Bots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: true),
                    Errors = table.Column<string>(type: "TEXT", nullable: true)
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
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
                    Errors = table.Column<string>(type: "TEXT", nullable: true),
                    DeltaPoints = table.Column<float>(type: "REAL", nullable: false),
                    Responsiveness = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_GameUsers", x => new { x.GameId, x.UserId });
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

            migrationBuilder.CreateTable(
                name: "App_Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_Tasks_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("476b9890-0d35-40f7-979e-b4f8f83e4e97"), new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(9801), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(9974), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("25ea522d-3e86-4805-b62f-2c4687226994"), new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(525), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(527), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("1633a219-e79d-4904-9b47-ef5a483a87f1"), new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(530), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(531), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("ef0078a9-e1ea-42c8-a341-78e70a34808f"), new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(533), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(534), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("f328e6d6-0bfe-41a7-8684-246bb1330f4b"), new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(536), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(537), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("ac1a9f72-9fe6-4fb9-935a-141cdb600b50"), new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(539), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(540), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("c092e249-b174-4eb5-bc56-9161480625b9"), new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(558), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 20, 2, 11, 44, 430, DateTimeKind.Utc).AddTicks(559), null });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(5448), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(5616), "Askaiser" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6183), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6184), "Falgar" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6185), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6186), "Xenure" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6188), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6188), "Minty" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6190), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6190), "Kalmera" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6192), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6193), "Pandarf" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6194), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 20, 2, 11, 44, 428, DateTimeKind.Utc).AddTicks(6195), "Mire" });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(5916), "", "", 1, new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6093), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6849), "", "", 1, new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6850), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6852), "", "", 1, new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6853), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6854), "", "", 1, new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6855), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6856), "", "", 1, new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6857), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6858), "", "", 1, new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6859), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6860), "", "", 1, new DateTime(2021, 2, 20, 2, 11, 44, 429, DateTimeKind.Utc).AddTicks(6861), new Guid("00000000-0000-0000-0000-000000000007") });

            migrationBuilder.CreateIndex(
                name: "IX_App_Bots_Updated",
                table: "App_Bots",
                column: "Updated");

            migrationBuilder.CreateIndex(
                name: "IX_App_Bots_UserId",
                table: "App_Bots",
                column: "UserId");

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
                name: "IX_App_Tasks_UserId",
                table: "App_Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_Email",
                table: "App_Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_GithubId",
                table: "App_Users",
                column: "GithubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_UserName",
                table: "App_Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_Bots");

            migrationBuilder.DropTable(
                name: "App_GameUsers");

            migrationBuilder.DropTable(
                name: "App_Tasks");

            migrationBuilder.DropTable(
                name: "App_Games");

            migrationBuilder.DropTable(
                name: "App_Users");
        }
    }
}
