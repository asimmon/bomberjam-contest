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
                name: "Users",
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
                    Errors = table.Column<string>(type: "TEXT", nullable: true),
                    DeltaPoints = table.Column<float>(type: "REAL", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Tasks",
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
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("cada3406-dc2a-4a40-a6e4-015a20fa6bd4"), new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(8972), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(8976), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("1a52d853-319c-483b-a1d6-f043a0a38a84"), new DateTime(2021, 2, 12, 23, 24, 58, 833, DateTimeKind.Utc).AddTicks(1172), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 24, 58, 833, DateTimeKind.Utc).AddTicks(1175), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("8a91e879-64b7-4a07-9c03-db142d0c8aac"), new DateTime(2021, 2, 12, 23, 24, 58, 833, DateTimeKind.Utc).AddTicks(1222), "00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 24, 58, 833, DateTimeKind.Utc).AddTicks(1223), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("910a5286-4f6f-405a-8d34-09aabf1d759e"), new DateTime(2021, 2, 12, 23, 24, 58, 833, DateTimeKind.Utc).AddTicks(1249), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000006:Pandarf", 1, 2, new DateTime(2021, 2, 12, 23, 24, 58, 833, DateTimeKind.Utc).AddTicks(1250), null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "", "", new DateTime(2021, 2, 12, 23, 24, 58, 830, DateTimeKind.Utc).AddTicks(9448), "simmon.anthony@gmail.com", 0, 14242083, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 24, 58, 830, DateTimeKind.Utc).AddTicks(9686), "Askaiser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "", "", new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1380), "falgar@gmail.com", 0, 36072624, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1382), "Falgar" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1385), "xenure@gmail.com", 0, 9208753, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1386), "Xenure" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), "", "", new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1387), "minty@gmail.com", 0, 26142591, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1388), "Minty" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), "", "", new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1389), "kalmera@gmail.com", 0, 5122918, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1390), "Kalmera" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), "", "", new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1391), "pandarf@gmail.com", 0, 1035273, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1392), "Pandarf" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), "", "", new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1393), "mire@gmail.com", 0, 5489330, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 24, 58, 831, DateTimeKind.Utc).AddTicks(1394), "Mire" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("e9d937cf-63cb-44dd-a3d1-895a128c3949"), new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(1205), "00000000-0000-0000-0000-000000000001", 1, 1, new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(1458), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("b996195a-3772-49e2-a1a1-480acf6d149f"), new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3632), "00000000-0000-0000-0000-000000000002", 1, 1, new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3635), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("a3c8af6d-aa68-4b22-8e63-331a36ec6a8e"), new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3641), "00000000-0000-0000-0000-000000000003", 1, 1, new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3641), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("fd4d90ae-420e-41de-87c0-2ac8cd01970a"), new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3644), "00000000-0000-0000-0000-000000000004", 1, 1, new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3644), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("f9983658-0d33-4f0c-b915-6be4daad3bfd"), new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3646), "00000000-0000-0000-0000-000000000005", 1, 1, new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3647), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("c7bdf232-c8d8-4567-9f96-a2eb13fd5bcb"), new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3649), "00000000-0000-0000-0000-000000000006", 1, 1, new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3650), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("ccbf6e0b-7309-456e-90c7-f796876efc31"), new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3652), "00000000-0000-0000-0000-000000000007", 1, 1, new DateTime(2021, 2, 12, 23, 24, 58, 832, DateTimeKind.Utc).AddTicks(3653), new Guid("00000000-0000-0000-0000-000000000007") });

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
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GithubId",
                table: "Users",
                column: "GithubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
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
