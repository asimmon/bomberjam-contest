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
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(8395), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(8565), "Askaiser" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9135), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9137), "Falgar" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9139), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9140), "Xenure" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9141), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9142), "Minty" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9144), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9145), "Kalmera" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9146), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9147), "Pandarf" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9148), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 20, 20, 33, 33, 954, DateTimeKind.Utc).AddTicks(9149), "Mire" });

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
