using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bomberjam.Website.Migrations
{
    public partial class AddSeasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "App_Games",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "App_Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_SeasonSummaries",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    RankedGameCount = table.Column<int>(type: "int", nullable: false),
                    GlobalRank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_SeasonSummaries", x => new { x.UserId, x.SeasonId });
                    table.ForeignKey(
                        name: "FK_App_SeasonSummaries_App_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "App_Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_App_SeasonSummaries_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "App_Seasons",
                columns: new[] { "Id", "Created", "Name", "Updated", "UserCount" },
                values: new object[] { 1, new DateTime(2021, 2, 28, 19, 0, 0, 0, DateTimeKind.Local), "S01", new DateTime(2021, 2, 28, 19, 0, 0, 0, DateTimeKind.Local), null });

            migrationBuilder.CreateIndex(
                name: "IX_App_Games_SeasonId",
                table: "App_Games",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_App_SeasonSummaries_SeasonId",
                table: "App_SeasonSummaries",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_App_Games_App_Seasons_SeasonId",
                table: "App_Games",
                column: "SeasonId",
                principalTable: "App_Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_App_Games_App_Seasons_SeasonId",
                table: "App_Games");

            migrationBuilder.DropTable(
                name: "App_SeasonSummaries");

            migrationBuilder.DropTable(
                name: "App_Seasons");

            migrationBuilder.DropIndex(
                name: "IX_App_Games_SeasonId",
                table: "App_Games");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "App_Games");
        }
    }
}
