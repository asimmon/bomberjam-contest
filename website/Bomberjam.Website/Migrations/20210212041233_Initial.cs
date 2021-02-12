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
                values: new object[] { new Guid("4b13ac24-d17d-461d-a4cf-661a0129cb7b"), new DateTime(2021, 2, 12, 4, 12, 33, 450, DateTimeKind.Utc).AddTicks(3), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 4, 12, 33, 450, DateTimeKind.Utc).AddTicks(8), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("320ca448-5ace-4958-b947-e828a5341846"), new DateTime(2021, 2, 12, 4, 12, 33, 450, DateTimeKind.Utc).AddTicks(1832), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 4, 12, 33, 450, DateTimeKind.Utc).AddTicks(1835), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("3a2fff15-b806-420f-aa68-706229218423"), new DateTime(2021, 2, 12, 4, 12, 33, 450, DateTimeKind.Utc).AddTicks(1859), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 12, 4, 12, 33, 450, DateTimeKind.Utc).AddTicks(1859), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("b9e40918-c3ac-4743-a421-ee620a7124f6"), new DateTime(2021, 2, 12, 4, 12, 33, 450, DateTimeKind.Utc).AddTicks(1873), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 4, 12, 33, 450, DateTimeKind.Utc).AddTicks(1874), null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "", "", new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(5700), "simmon.anthony@gmail.com", 0, 14242083, false, false, 1500f, 1, new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(5873), "Askaiser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "", "", new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7413), "falgar@gmail.com", 0, 36072624, false, false, 1500f, 1, new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7417), "Falgar" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7420), "xenure@gmail.com", 0, 9208753, false, false, 1500f, 1, new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7421), "Xenure" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), "", "", new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7422), "minty@gmail.com", 0, 26142591, false, false, 1500f, 1, new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7423), "Minty" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), "", "", new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7425), "kalmera@gmail.com", 0, 5122918, false, false, 1500f, 1, new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7426), "Kalmera" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), "", "", new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7428), "pandarf@gmail.com", 0, 1035273, false, false, 1500f, 1, new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7429), "Pandarf" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), "", "", new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7431), "mire@gmail.com", 0, 5489330, false, false, 1500f, 1, new DateTime(2021, 2, 12, 4, 12, 33, 448, DateTimeKind.Utc).AddTicks(7432), "Mire" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("78e41df7-7bc3-4815-b8b3-816c08b49851"), new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(6671), "00000000-0000-0000-0000-000000000001", 1, 1, new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(6852), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("5ecb4629-5b69-4df2-a6f6-2378b89f3824"), new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7856), "00000000-0000-0000-0000-000000000002", 1, 1, new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7858), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("c32bf946-9127-490d-9de8-7905f594db97"), new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7862), "00000000-0000-0000-0000-000000000003", 1, 1, new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7863), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("03b0f003-702a-4dee-a3db-f80c55b7d5b9"), new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7866), "00000000-0000-0000-0000-000000000004", 1, 1, new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7867), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("36fd6622-b47e-446b-a859-7d77b61e5287"), new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7869), "00000000-0000-0000-0000-000000000005", 1, 1, new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7870), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("fe64b2d5-52c6-4df1-af97-fa66c3b8b346"), new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7872), "00000000-0000-0000-0000-000000000006", 1, 1, new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7873), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("b850786b-f8a1-4fae-9dd9-e6ecbf5f5f27"), new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7876), "00000000-0000-0000-0000-000000000007", 1, 1, new DateTime(2021, 2, 12, 4, 12, 33, 449, DateTimeKind.Utc).AddTicks(7876), new Guid("00000000-0000-0000-0000-000000000007") });

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
