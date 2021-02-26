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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitDuration = table.Column<double>(type: "float", nullable: true),
                    GameDuration = table.Column<double>(type: "float", nullable: true),
                    Stdout = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stderr = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GithubId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Points = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Bots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeltaPoints = table.Column<float>(type: "real", nullable: false),
                    Responsiveness = table.Column<float>(type: "real", nullable: false)
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                values: new object[,]
                {
                    { new Guid("1ca68972-7ecb-446a-9803-3832a1f58c90"), new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(940), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(1180), null },
                    { new Guid("ed2a86a3-674e-4a40-85f3-446b1273d016"), new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(1998), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2001), null },
                    { new Guid("2ce8a06a-6438-4a1a-aa4c-a09edbc7b96e"), new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2005), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2006), null },
                    { new Guid("85f51f94-6e23-4ccb-ae23-e13460abd497"), new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2008), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2009), null },
                    { new Guid("72e6044b-bf11-4996-ae1a-4016bda9fedc"), new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2011), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2011), null },
                    { new Guid("443b71a5-3f7b-4427-b7f5-929d73522b9b"), new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2013), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2014), null },
                    { new Guid("efced929-e8e1-44ac-a83e-132e3a8a0c5d"), new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2027), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 26, 0, 53, 21, 388, DateTimeKind.Utc).AddTicks(2028), null }
                });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(4146), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(4384), "Askaiser" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5168), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5171), "Falgar" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5173), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5174), "Xenure" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5175), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5176), "Minty" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5177), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5178), "Kalmera" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5179), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5179), "Pandarf" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5182), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 26, 0, 53, 21, 386, DateTimeKind.Utc).AddTicks(5182), "Mire" }
                });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(6160), "", "", 1, new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(6410), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7662), "", "", 1, new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7664), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7667), "", "", 1, new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7667), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7668), "", "", 1, new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7669), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7670), "", "", 1, new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7671), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7672), "", "", 1, new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7673), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7674), "", "", 1, new DateTime(2021, 2, 26, 0, 53, 21, 387, DateTimeKind.Utc).AddTicks(7675), new Guid("00000000-0000-0000-0000-000000000007") }
                });

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
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_GithubId",
                table: "App_Users",
                column: "GithubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_App_Users_UserName",
                table: "App_Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
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
