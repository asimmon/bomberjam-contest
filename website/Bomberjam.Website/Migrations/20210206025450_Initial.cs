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
                    Rank = table.Column<int>(type: "INTEGER", nullable: false),
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
                values: new object[] { new Guid("566d290a-2263-49de-ae13-53dee2fbab4f"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(2891), "00000000-0000-0000-0000-000000000001", 1, 1, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3094) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("f7f5286d-9feb-4dbf-80e0-18f39e5b5427"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(7623), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(7624) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("9ef6f451-ade7-47d6-9c6f-f7d7b50f8cec"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(7607), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(7608) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("889f8c20-09a1-44fd-834e-feb2ee989c7c"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(5667), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(5671) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("b443cd5a-6b5b-4787-9565-2909fb376347"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3743), "00000000-0000-0000-0000-000000000007", 1, 1, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3744) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("2fb9fbb4-6b66-4e91-8430-6d2491ba2412"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(7564), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(7568) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("77b9a817-7e8c-4fbd-90e3-89a506167301"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3736), "00000000-0000-0000-0000-000000000005", 1, 1, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3737) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("98dd3f0e-70cc-488d-bc3e-c7fb330fb0d7"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3733), "00000000-0000-0000-0000-000000000004", 1, 1, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3734) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("766ca326-810c-49f3-9666-5297eb190478"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3730), "00000000-0000-0000-0000-000000000003", 1, 1, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3731) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("69a8d5ad-e558-4e53-af6b-162d1fa08eff"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3725), "00000000-0000-0000-0000-000000000002", 1, 1, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3726) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("f4e98337-29a0-47cb-8026-9a1077a0b864"), new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3740), "00000000-0000-0000-0000-000000000006", 1, 1, new DateTime(2021, 2, 6, 2, 54, 50, 153, DateTimeKind.Utc).AddTicks(3740) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), "", "", new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1948), "pandarf@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1949), "Pandarf" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "", "", new DateTime(2021, 2, 6, 2, 54, 50, 151, DateTimeKind.Utc).AddTicks(9672), "simmon.anthony@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 6, 2, 54, 50, 151, DateTimeKind.Utc).AddTicks(9949), "Askaiser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "", "", new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1934), "falgar@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1938), "Falgar" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1941), "xenure@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1942), "Xenure" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), "", "", new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1943), "minty@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1944), "Minty" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), "", "", new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1946), "kalmera@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1947), "Kalmera" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), "", "", new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1951), "mire@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 6, 2, 54, 50, 152, DateTimeKind.Utc).AddTicks(1952), "Mire" });

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
