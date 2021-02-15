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
                    SubmitCount = table.Column<int>(type: "INTEGER", nullable: false),
                    GameCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCompiling = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsCompiled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompilationErrors = table.Column<string>(type: "TEXT", nullable: true),
                    BotLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    Points = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Users", x => x.Id);
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
                values: new object[] { new Guid("ebd0d6cd-af5d-455a-9b06-bd754f89d158"), new DateTime(2021, 2, 15, 7, 51, 37, 987, DateTimeKind.Utc).AddTicks(1960), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 15, 7, 51, 37, 987, DateTimeKind.Utc).AddTicks(1963), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("2c474ca3-3730-4b10-b33a-20a83d3ad20f"), new DateTime(2021, 2, 15, 7, 51, 37, 987, DateTimeKind.Utc).AddTicks(3743), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 15, 7, 51, 37, 987, DateTimeKind.Utc).AddTicks(3746), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("6e8ea553-b695-4e21-aeae-4b681c5c8384"), new DateTime(2021, 2, 15, 7, 51, 37, 987, DateTimeKind.Utc).AddTicks(3816), "00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 15, 7, 51, 37, 987, DateTimeKind.Utc).AddTicks(3816), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("ea140403-0167-44f2-a93a-e9d02b719013"), new DateTime(2021, 2, 15, 7, 51, 37, 987, DateTimeKind.Utc).AddTicks(3851), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000006:Pandarf", 1, 2, new DateTime(2021, 2, 15, 7, 51, 37, 987, DateTimeKind.Utc).AddTicks(3852), null });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "", "", new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(5698), "simmon.anthony@gmail.com", 0, 14242083, false, false, 1500f, 1, new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(5869), "Askaiser" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "", "", new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7160), "falgar@gmail.com", 0, 36072624, false, false, 1500f, 1, new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7163), "Falgar" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7165), "xenure@gmail.com", 0, 9208753, false, false, 1500f, 1, new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7166), "Xenure" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), "", "", new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7167), "minty@gmail.com", 0, 26142591, false, false, 1500f, 1, new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7168), "Minty" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), "", "", new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7169), "kalmera@gmail.com", 0, 5122918, false, false, 1500f, 1, new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7170), "Kalmera" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), "", "", new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7171), "pandarf@gmail.com", 0, 1035273, false, false, 1500f, 1, new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7172), "Pandarf" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), "", "", new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7174), "mire@gmail.com", 0, 5489330, false, false, 1500f, 1, new DateTime(2021, 2, 15, 7, 51, 37, 985, DateTimeKind.Utc).AddTicks(7174), "Mire" });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("f7ac849d-d059-4286-a56a-ab25e144db7e"), new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(6157), "00000000-0000-0000-0000-000000000001", 1, 1, new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(6339), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("89a22c17-0406-4b80-9c7f-d5f657f71392"), new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7545), "00000000-0000-0000-0000-000000000002", 1, 1, new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7547), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("d578c9e7-a796-4982-ab50-16ddf9cd936d"), new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7551), "00000000-0000-0000-0000-000000000003", 1, 1, new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7552), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("94990982-86ba-4be3-b62a-985475ee2813"), new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7555), "00000000-0000-0000-0000-000000000004", 1, 1, new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7555), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("9c7599f5-d1fc-4815-bdb3-379c6cd6f3bf"), new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7558), "00000000-0000-0000-0000-000000000005", 1, 1, new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7559), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("84546e20-5b80-4e5d-881d-1fae354da207"), new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7561), "00000000-0000-0000-0000-000000000006", 1, 1, new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7562), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("22b810ad-2cf3-4c76-8651-078921a1cfdf"), new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7564), "00000000-0000-0000-0000-000000000007", 1, 1, new DateTime(2021, 2, 15, 7, 51, 37, 986, DateTimeKind.Utc).AddTicks(7565), new Guid("00000000-0000-0000-0000-000000000007") });

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
