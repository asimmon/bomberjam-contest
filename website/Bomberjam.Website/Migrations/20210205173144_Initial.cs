using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bomberjam.Website.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WinnerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Errors = table.Column<string>(type: "TEXT", nullable: true),
                    InitDuration = table.Column<double>(type: "REAL", nullable: true),
                    GameDuration = table.Column<double>(type: "REAL", nullable: true),
                    Stdout = table.Column<string>(type: "TEXT", nullable: true),
                    Stderr = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    SubmitCount = table.Column<int>(type: "INTEGER", nullable: false),
                    GameCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCompiling = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsCompiled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompilationErrors = table.Column<string>(type: "TEXT", nullable: true),
                    BotLanguage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameUsers",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    Errors = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUsers", x => new { x.GameId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GameUsers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("b9f80ecb-892d-40f6-9adb-33c7eeff5bab"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(1553), "00000000-0000-0000-0000-000000000001", 1, 1, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(1818) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("4c3dd66e-b893-4f79-a615-04c18a76021a"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(6411), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(6411) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("e4f36de0-94ab-4a66-93da-a455871b46b4"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(6401), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(6402) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("dd5c24cb-8566-41c9-8b42-8461ccfbc905"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(4359), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(4362) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("360f6a9d-40dc-44a0-9750-24af09ed207e"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2590), "00000000-0000-0000-0000-000000000007", 1, 1, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2591) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("9cc6ec17-e25b-4c0d-a628-99c7148ab50c"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(6377), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(6381) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("72faa54e-93ea-48b5-b60f-ba7ca4b43f77"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2585), "00000000-0000-0000-0000-000000000005", 1, 1, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2586) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("6a719ad0-b897-40dc-b9ba-cedf6c4a6846"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2582), "00000000-0000-0000-0000-000000000004", 1, 1, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2583) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("3a16c227-e0d8-4094-bad1-1f81dea6e72d"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2579), "00000000-0000-0000-0000-000000000003", 1, 1, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("4fa73c89-50c2-4001-ab2e-e533d53f09f0"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2573), "00000000-0000-0000-0000-000000000002", 1, 1, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2575) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("4e36d430-4fcb-4a7d-baa0-dd6a88fa25da"), new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2588), "00000000-0000-0000-0000-000000000006", 1, 1, new DateTime(2021, 2, 5, 17, 31, 44, 474, DateTimeKind.Utc).AddTicks(2588) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), "", "", new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1962), "pandarf@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1962), "Pandarf" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "", "", new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(2), "simmon.anthony@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(258), "Askaiser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "", "", new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1951), "falgar@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1954), "Falgar" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1956), "xenure@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1957), "Xenure" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), "", "", new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1958), "minty@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1959), "Minty" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), "", "", new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1960), "kalmera@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1961), "Kalmera" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), "", "", new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1964), "mire@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 17, 31, 44, 473, DateTimeKind.Utc).AddTicks(1964), "Mire" });

            migrationBuilder.CreateIndex(
                name: "IX_GameUsers_UserId",
                table: "GameUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Created",
                table: "Tasks",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status",
                table: "Tasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Type",
                table: "Tasks",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameUsers");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
