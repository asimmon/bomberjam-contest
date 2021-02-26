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
                    DeltaPoints = table.Column<float>(type: "REAL", nullable: false)
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
                values: new object[] { new Guid("a73f6f1b-ec50-4d33-ab28-348157732162"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(7391), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(7568), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("2637ca15-71b7-4553-9750-e9ff5a54df11"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8124), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8127), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("fffccffa-c16c-4d24-82e5-54ed92336474"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8129), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8130), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("f714f423-aa6c-498f-bf50-eac42a6607c4"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8132), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8133), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("7daac732-1892-403d-a95b-eae59fed73e3"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8136), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8136), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("10fc77a9-bcce-4345-8264-716652f08cca"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8153), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8154), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("1a370401-e716-4df2-989f-302f5dbbf937"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8157), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(8157), null });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(2512), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(2685), "Askaiser" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3300), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3302), "Falgar" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3304), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3305), "Xenure" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3306), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3307), "Minty" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3308), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3309), "Kalmera" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3310), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3311), "Pandarf" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3312), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 26, 5, 3, 13, 134, DateTimeKind.Utc).AddTicks(3313), "Mire" });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(3133), "", "", 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(3318), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4083), "", "", 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4085), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4087), "", "", 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4088), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4127), "", "", 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4128), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4130), "", "", 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4130), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4132), "", "", 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4133), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4134), "", "", 1, new DateTime(2021, 2, 26, 5, 3, 13, 135, DateTimeKind.Utc).AddTicks(4135), new Guid("00000000-0000-0000-0000-000000000007") });

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
