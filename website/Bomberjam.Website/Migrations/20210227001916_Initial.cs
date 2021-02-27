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
                    DeltaPoints = table.Column<float>(type: "real", nullable: false)
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
                    { new Guid("8830b01a-d3d3-47ea-b458-3d94b974b8e5"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(3440), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(3611), null },
                    { new Guid("890361c1-ce13-47bc-a6df-85b1f21a983e"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4287), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4289), null },
                    { new Guid("0d41111e-a656-4eaf-bfe5-74939913474b"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4293), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4294), null },
                    { new Guid("fa741bc1-badf-4b1c-8c72-9add88d11424"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4297), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4298), null },
                    { new Guid("76eab766-af87-4cf1-8344-e15a1402c6c9"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4300), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4301), null },
                    { new Guid("ff5fefd7-a426-4f69-bac6-c8d6de376b9c"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4303), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4304), null },
                    { new Guid("f71a35ac-0d53-48bd-a479-e4148fd8201c"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4307), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(4308), null }
                });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(8721), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(8891), "Askaiser" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9573), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9574), "Falgar" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9576), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9577), "Xenure" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9578), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9579), "Minty" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9580), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9581), "Kalmera" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9582), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9583), "Pandarf" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9584), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 27, 0, 19, 16, 179, DateTimeKind.Utc).AddTicks(9585), "Mire" }
                });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 27, 0, 19, 16, 180, DateTimeKind.Utc).AddTicks(9435), "", "", 1, new DateTime(2021, 2, 27, 0, 19, 16, 180, DateTimeKind.Utc).AddTicks(9613), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(444), "", "", 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(449), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(451), "", "", 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(451), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(453), "", "", 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(454), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(455), "", "", 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(456), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(457), "", "", 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(458), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(459), "", "", 1, new DateTime(2021, 2, 27, 0, 19, 16, 181, DateTimeKind.Utc).AddTicks(460), new Guid("00000000-0000-0000-0000-000000000007") }
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
