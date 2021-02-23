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
                values: new object[] { new Guid("84c80409-f8a7-4fa5-94f1-2b4841d97056"), new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(1868), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(2214), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("8bdd023b-2c32-440f-b2af-96c56db4594c"), new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3006), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3007), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("a2b02f33-82f0-428b-899a-f8816e272607"), new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3012), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3013), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("83baa9b0-ddf1-4d92-b46a-38dd3d954913"), new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3014), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3015), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("e6a62b3e-1e00-4fba-8273-c075b9881304"), new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3018), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3018), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("5ab114b1-fc27-4567-8ac6-ccb40acd203f"), new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3020), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3021), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("e8e6ad71-f191-42e4-8559-62b4966bc98f"), new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3023), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 23, 16, 49, 35, 169, DateTimeKind.Utc).AddTicks(3023), null });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(5075), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(5322), "Askaiser" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6083), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6085), "Falgar" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6086), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6087), "Xenure" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6088), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6088), "Minty" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6089), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6090), "Kalmera" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6091), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6092), "Pandarf" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6093), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 23, 16, 49, 35, 167, DateTimeKind.Utc).AddTicks(6094), "Mire" });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(7145), "", "", 1, new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(7395), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8441), "", "", 1, new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8443), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8445), "", "", 1, new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8446), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8447), "", "", 1, new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8447), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8448), "", "", 1, new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8449), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8450), "", "", 1, new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8451), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8452), "", "", 1, new DateTime(2021, 2, 23, 16, 49, 35, 168, DateTimeKind.Utc).AddTicks(8452), new Guid("00000000-0000-0000-0000-000000000007") });

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
