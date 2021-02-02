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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsFinished = table.Column<bool>(type: "INTEGER", nullable: false),
                    WinnerId = table.Column<int>(type: "INTEGER", nullable: true),
                    Errors = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
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
                values: new object[] { 1, new DateTime(2021, 2, 2, 3, 19, 54, 984, DateTimeKind.Utc).AddTicks(7154), "1", 1, 1, new DateTime(2021, 2, 2, 3, 19, 54, 984, DateTimeKind.Utc).AddTicks(7340) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { 2, new DateTime(2021, 2, 2, 3, 19, 54, 984, DateTimeKind.Utc).AddTicks(7951), "2", 1, 1, new DateTime(2021, 2, 2, 3, 19, 54, 984, DateTimeKind.Utc).AddTicks(7953) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { 3, new DateTime(2021, 2, 2, 3, 19, 54, 984, DateTimeKind.Utc).AddTicks(7955), "3", 1, 1, new DateTime(2021, 2, 2, 3, 19, 54, 984, DateTimeKind.Utc).AddTicks(7956) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated" },
                values: new object[] { 4, new DateTime(2021, 2, 2, 3, 19, 54, 984, DateTimeKind.Utc).AddTicks(7988), "4", 1, 1, new DateTime(2021, 2, 2, 3, 19, 54, 984, DateTimeKind.Utc).AddTicks(7989) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { 1, "", "", new DateTime(2021, 2, 2, 3, 19, 54, 983, DateTimeKind.Utc).AddTicks(7321), "simmon.anthony@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 2, 3, 19, 54, 983, DateTimeKind.Utc).AddTicks(7510), "Askaiser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { 2, "", "", new DateTime(2021, 2, 2, 3, 19, 54, 983, DateTimeKind.Utc).AddTicks(8676), "falgar@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 2, 3, 19, 54, 983, DateTimeKind.Utc).AddTicks(8678), "Falgar" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { 3, "", "", new DateTime(2021, 2, 2, 3, 19, 54, 983, DateTimeKind.Utc).AddTicks(8680), "xenure@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 2, 3, 19, 54, 983, DateTimeKind.Utc).AddTicks(8681), "Xenure" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "IsCompiled", "IsCompiling", "SubmitCount", "Updated", "Username" },
                values: new object[] { 4, "", "", new DateTime(2021, 2, 2, 3, 19, 54, 983, DateTimeKind.Utc).AddTicks(8683), "minty@gmail.com", 0, false, false, 1, new DateTime(2021, 2, 2, 3, 19, 54, 983, DateTimeKind.Utc).AddTicks(8683), "Minty" });

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
