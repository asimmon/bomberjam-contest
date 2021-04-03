using Microsoft.EntityFrameworkCore.Migrations;

namespace Bomberjam.Website.Migrations
{
    public partial class AddUserGlobalRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GlobalRank",
                table: "App_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlobalRank",
                table: "App_Users");
        }
    }
}
