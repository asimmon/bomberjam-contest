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
                values: new object[] { new Guid("a67e6efd-de5a-4424-99f9-54393795f8fe"), new DateTime(2021, 2, 5, 0, 15, 29, 833, DateTimeKind.Utc).AddTicks(8460), "00000000-0000-0000-0000-000000000001", 1, 1, new DateTime(2021, 2, 5, 0, 15, 29, 833, DateTimeKind.Utc).AddTicks(8657) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("93777e82-a26d-475d-89ce-682f9c55269f"), new DateTime(2021, 2, 5, 0, 15, 29, 833, DateTimeKind.Utc).AddTicks(9250), "00000000-0000-0000-0000-000000000002", 1, 1, new DateTime(2021, 2, 5, 0, 15, 29, 833, DateTimeKind.Utc).AddTicks(9252) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("637685ac-f0c0-4201-9b5a-89c48ac7d066"), new DateTime(2021, 2, 5, 0, 15, 29, 833, DateTimeKind.Utc).AddTicks(9256), "00000000-0000-0000-0000-000000000003", 1, 1, new DateTime(2021, 2, 5, 0, 15, 29, 833, DateTimeKind.Utc).AddTicks(9257) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { new Guid("93fa9939-c6f4-4538-89d8-25c7d60f7ae3"), new DateTime(2021, 2, 5, 0, 15, 29, 833, DateTimeKind.Utc).AddTicks(9259), "00000000-0000-0000-0000-000000000004", 1, 1, new DateTime(2021, 2, 5, 0, 15, 29, 833, DateTimeKind.Utc).AddTicks(9260) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "", "", new DateTime(2021, 2, 5, 0, 15, 29, 832, DateTimeKind.Utc).AddTicks(8471), "simmon.anthony@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 0, 15, 29, 832, DateTimeKind.Utc).AddTicks(8660), "Askaiser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "", "", new DateTime(2021, 2, 5, 0, 15, 29, 832, DateTimeKind.Utc).AddTicks(9825), "falgar@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 0, 15, 29, 832, DateTimeKind.Utc).AddTicks(9827), "Falgar" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2021, 2, 5, 0, 15, 29, 832, DateTimeKind.Utc).AddTicks(9830), "xenure@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 0, 15, 29, 832, DateTimeKind.Utc).AddTicks(9831), "Xenure" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), "", "", new DateTime(2021, 2, 5, 0, 15, 29, 832, DateTimeKind.Utc).AddTicks(9834), "minty@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 5, 0, 15, 29, 832, DateTimeKind.Utc).AddTicks(9835), "Minty" });

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
