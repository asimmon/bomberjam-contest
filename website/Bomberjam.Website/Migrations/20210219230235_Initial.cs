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
                    DeltaPoints = table.Column<float>(type: "REAL", nullable: false)
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
                values: new object[] { new Guid("7f6d1154-a11a-47f0-88f2-2eaf76905ef3"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9092), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9261), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("9a856208-f40a-417a-9343-b0e2ce9ed6c1"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9964), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9966), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("353f5522-82b7-4cb7-8d58-681094388226"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9970), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9971), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("284ca368-048f-4971-8242-0a414756feff"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9973), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9974), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("d8e16ffa-611a-4ec5-b244-0eaef17b95fb"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9976), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9977), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("bc570354-53af-4bab-924d-14189029ee33"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9979), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9980), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("3c375212-b83c-447a-b3db-d9532c8b0785"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9983), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(9983), null });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(4620), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(4799), "Askaiser" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5448), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5450), "Falgar" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5452), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5453), "Xenure" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5454), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5455), "Minty" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5456), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5457), "Kalmera" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5459), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5460), "Pandarf" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5461), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 19, 23, 2, 35, 309, DateTimeKind.Utc).AddTicks(5462), "Mire" });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(5270), "", "", 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(5446), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6197), "", "", 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6199), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6201), "", "", 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6202), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6203), "", "", 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6204), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6205), "", "", 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6206), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6207), "", "", 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6208), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6210), "", "", 1, new DateTime(2021, 2, 19, 23, 2, 35, 310, DateTimeKind.Utc).AddTicks(6210), new Guid("00000000-0000-0000-0000-000000000007") });

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
