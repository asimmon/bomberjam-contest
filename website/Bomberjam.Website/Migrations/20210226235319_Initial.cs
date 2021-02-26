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
                    { new Guid("61fb772f-2c6c-42d8-815b-acfd18dcb27c"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3122), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3306), null },
                    { new Guid("76999883-ae54-4911-87be-3954b991c4cd"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3911), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3912), null },
                    { new Guid("0e0ddffd-fb85-47f4-beda-d14a74cd1893"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3916), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3917), null },
                    { new Guid("1d606580-54f2-4b00-9a1e-9aa1f4460aa3"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3919), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3920), null },
                    { new Guid("c597e25e-3dfa-42f4-ae04-c783f8c47c18"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3922), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3923), null },
                    { new Guid("38d069b4-c776-4d39-9a02-54384d3d33b0"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3926), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3927), null },
                    { new Guid("0110fa15-ddb6-43ff-b910-597900c7ffd4"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3929), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(3930), null }
                });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8095), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8274), "Askaiser" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8940), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8943), "Falgar" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8945), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8945), "Xenure" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8947), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8948), "Minty" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8949), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8950), "Kalmera" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8951), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8952), "Pandarf" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8954), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 26, 23, 53, 18, 925, DateTimeKind.Utc).AddTicks(8954), "Mire" }
                });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 26, 23, 53, 18, 926, DateTimeKind.Utc).AddTicks(8976), "", "", 1, new DateTime(2021, 2, 26, 23, 53, 18, 926, DateTimeKind.Utc).AddTicks(9162), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(69), "", "", 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(75), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(77), "", "", 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(78), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(80), "", "", 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(81), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(82), "", "", 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(83), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(84), "", "", 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(85), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(86), "", "", 1, new DateTime(2021, 2, 26, 23, 53, 18, 927, DateTimeKind.Utc).AddTicks(87), new Guid("00000000-0000-0000-0000-000000000007") }
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
