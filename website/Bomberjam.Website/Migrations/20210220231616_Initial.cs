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
                values: new object[] { new Guid("dcd6224d-b400-45c4-9194-af4f9a22192a"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5096), "00000000-0000-0000-0000-000000000001", 0, 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5280), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("00361298-3e6f-40bf-9425-ae3d74556c40"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5867), "00000000-0000-0000-0000-000000000002", 0, 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5869), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("96ec2f73-d2f2-4955-8b35-9b36785262ae"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5872), "00000000-0000-0000-0000-000000000003", 0, 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5873), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("3cd9fa31-d093-47e8-8ef9-ee76081d19c0"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5875), "00000000-0000-0000-0000-000000000004", 0, 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5876), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("764cab38-1510-4c6f-b4e8-36d0bcad141b"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5878), "00000000-0000-0000-0000-000000000005", 0, 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5879), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("35582969-b6cb-4ccd-b2d0-9e8d0a69113f"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5881), "00000000-0000-0000-0000-000000000006", 0, 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5882), null });

            migrationBuilder.InsertData(
                table: "App_Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("de5ee213-11a8-4d28-be51-11955365df64"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5884), "00000000-0000-0000-0000-000000000007", 0, 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(5885), null });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(281), "simmon.anthony@gmail.com", 14242083, 1500f, new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(460), "Askaiser" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1055), "falgar@gmail.com", 36072624, 1500f, new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1057), "Falgar" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1058), "xenure@gmail.com", 9208753, 1500f, new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1059), "Xenure" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1060), "minty@gmail.com", 26142591, 1500f, new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1061), "Minty" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1063), "kalmera@gmail.com", 5122918, 1500f, new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1063), "Kalmera" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1065), "pandarf@gmail.com", 1035273, 1500f, new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1066), "Pandarf" });

            migrationBuilder.InsertData(
                table: "App_Users",
                columns: new[] { "Id", "Created", "Email", "GithubId", "Points", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1067), "mire@gmail.com", 5489330, 1500f, new DateTime(2021, 2, 20, 23, 16, 16, 564, DateTimeKind.Utc).AddTicks(1068), "Mire" });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(982), "", "", 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1169), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1983), "", "", 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1985), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1987), "", "", 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1988), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1989), "", "", 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1990), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1991), "", "", 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1992), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1993), "", "", 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1994), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "App_Bots",
                columns: new[] { "Id", "Created", "Errors", "Language", "Status", "Updated", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1995), "", "", 1, new DateTime(2021, 2, 20, 23, 16, 16, 565, DateTimeKind.Utc).AddTicks(1996), new Guid("00000000-0000-0000-0000-000000000007") });

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
