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
                values: new object[] { new Guid("4aecc989-9e3e-49b7-bfab-32cb0230dab8"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(2331), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(2333), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("50a80a23-1012-4de3-b44b-f26825fd61b7"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5254), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5255), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("1a2ecf25-4fea-42bd-9232-5dad00083040"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5313), "00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000006:Pandarf", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5314), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("0a7356e9-af39-40d9-adff-19c87e7d2db8"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5343), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5344), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("60be299d-80ee-40c5-8de7-85809d12059d"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5401), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000007:Mire", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5402), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("3343d212-51e8-4916-8ec2-aef737a2decc"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5433), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5434), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("82559707-bdaa-4dc6-90bc-9c23b736ec37"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5463), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000004:Minty", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5464), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("0c4a5b42-330c-4d6c-8254-a106aa1102c8"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5492), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000007:Mire", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5493), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("4517fb8d-0fa8-4622-99ca-ab938a9af665"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5522), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5523), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("fc9a0977-bcad-48e8-952d-342cac2b2136"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5582), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5583), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("8feaabc6-4674-4862-a80f-78842769f58a"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5612), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5613), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("26eb0078-51e9-41d5-8b31-a50501156646"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5224), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5225), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("3811824b-5171-4a25-84f7-13ca00304170"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5642), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5642), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("752d9b0a-a95a-47e5-b3bd-6ffe2f4bb71b"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5703), "00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000006:Pandarf", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5704), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("27041e4b-d843-4931-b9ae-a2bedbac649f"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5733), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5734), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("a0fae483-e7ee-4c58-8628-d90a4f5b10df"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5793), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5794), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("6cae45f4-d7b7-4913-9f3b-76a6cac6a104"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5823), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5823), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("adcffcc7-466c-406f-bae5-634e7c42705e"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5852), "00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5853), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("8f7def3c-4005-4a8b-b69a-c199e8d552ef"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5882), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5883), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("7ae497b6-3b2d-4f1c-ad1d-0044a890ed18"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5912), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5912), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("359acd8a-f0c4-458a-85fa-bc121c656b2e"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5973), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000007:Mire", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5974), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("a696c671-d34e-4512-96ee-490d86428063"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(6005), "00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000007:Mire", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(6006), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("87a0bd9b-2eaa-441c-890f-04fb575d7fd1"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(6035), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(6036), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("11d1087f-e45f-4b34-9258-859bfdeb5699"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5671), "00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5672), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("fafad927-9753-4739-88de-e94d56a0fb09"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5154), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5155), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("d55b04e9-d733-4547-bb38-b22b423dc3fe"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5284), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5285), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("ea812db3-9f19-4d6a-9da5-373d577e59d7"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5091), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000004:Minty", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5092), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("5ff16427-0feb-49dd-a4ab-3d4337d5423b"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4119), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4122), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("05a9e2ab-ef26-46e8-99ca-26a0d0398523"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4169), "00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4170), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("e24a1bb0-6dc5-45ab-b30e-97c68d4c1189"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4201), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000006:Pandarf", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4202), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("0cb01ff3-a5cd-4e5f-80fd-58eca73cda61"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4271), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4272), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("4e5cb5c6-4cc4-4ad7-9b97-b17752e143c6"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4344), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4345), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("5eced64e-22d1-41c2-ab49-a08d4b25a060"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4375), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4376), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("a2aa5c14-ba6d-4ab2-82aa-755b03af7010"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4464), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4465), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("5e1fc41d-a1f3-4914-83d4-6de2dc41a70e"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4500), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4501), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("ff665674-1dff-4270-be05-76baeacd65f2"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5124), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5125), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("931d8660-ec5b-4956-9542-19aef0050e69"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4610), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4611), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("2752f5d0-0579-4e59-ab3d-d246f6928149"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4640), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4641), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("d8fe2a7f-e079-4c86-9036-cbe62317905d"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4576), "00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4577), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("30db36be-6790-422d-be99-50d1bc669ee3"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4723), "00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4724), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("87fb315c-718b-4fae-ae90-dcfa4c95b56d"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5061), "00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5062), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("7cc2c16c-d223-47e4-8fbf-2312368fa6ff"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4693), "00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000005:Kalmera", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4694), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("e8fe6621-f5f9-4e05-b2ea-692c665865d2"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5001), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000006:Pandarf", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5002), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("78f59c57-03c1-4d9e-b83e-092a5834b5e5"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4940), "00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4941), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("025be423-7916-4159-8167-39c0bd0423db"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5031), "00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000002:Falgar", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(5032), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("219d68f1-cde4-4fc6-86fa-d407df6750de"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4880), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000007:Mire", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4881), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("96f2a857-8085-4d6b-a49a-192458fc2447"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4850), "00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000007:Mire", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4851), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("02444894-d838-4952-a969-0a85dada15eb"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4816), "00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000007:Mire,00000000-0000-0000-0000-000000000001:Askaiser,00000000-0000-0000-0000-000000000003:Xenure", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4817), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("119d0fdf-481c-41fc-9d18-2ec59366956b"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4753), "00000000-0000-0000-0000-000000000005:Kalmera,00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000004:Minty,00000000-0000-0000-0000-000000000006:Pandarf", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4754), null });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("6a1fda96-8773-4487-aedd-eff01d4c4fa8"), new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4910), "00000000-0000-0000-0000-000000000002:Falgar,00000000-0000-0000-0000-000000000003:Xenure,00000000-0000-0000-0000-000000000006:Pandarf,00000000-0000-0000-0000-000000000001:Askaiser", 1, 2, new DateTime(2021, 2, 12, 23, 39, 22, 966, DateTimeKind.Utc).AddTicks(4911), null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000006"), "", "", new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8144), "pandarf@gmail.com", 0, 1035273, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8145), "Pandarf" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "", "", new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(6549), "simmon.anthony@gmail.com", 0, 14242083, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(6727), "Askaiser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000002"), "", "", new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8133), "falgar@gmail.com", 0, 36072624, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8135), "Falgar" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000003"), "", "", new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8137), "xenure@gmail.com", 0, 9208753, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8138), "Xenure" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000004"), "", "", new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8139), "minty@gmail.com", 0, 26142591, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8140), "Minty" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000005"), "", "", new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8142), "kalmera@gmail.com", 0, 5122918, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8142), "Kalmera" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BotLanguage", "CompilationErrors", "Created", "Email", "GameCount", "GithubId", "IsCompiled", "IsCompiling", "Points", "SubmitCount", "Updated", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000007"), "", "", new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8146), "mire@gmail.com", 0, 5489330, false, false, 1500f, 1, new DateTime(2021, 2, 12, 23, 39, 22, 964, DateTimeKind.Utc).AddTicks(8147), "Mire" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("20cba268-24e7-47f5-804f-0a8948de9864"), new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(7110), "00000000-0000-0000-0000-000000000001", 1, 1, new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(7295), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("8b288526-d5b9-436d-aae1-67b08b9547e8"), new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8330), "00000000-0000-0000-0000-000000000002", 1, 1, new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8331), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("a2787561-fbdc-4ec1-bc0e-7fd75cd20c13"), new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8336), "00000000-0000-0000-0000-000000000003", 1, 1, new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8336), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("913a7188-5703-47e2-8902-e36a7bd833d9"), new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8339), "00000000-0000-0000-0000-000000000004", 1, 1, new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8340), new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("c1678711-d2d8-4fa9-8553-eace62bb6e1b"), new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8342), "00000000-0000-0000-0000-000000000005", 1, 1, new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8343), new Guid("00000000-0000-0000-0000-000000000005") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("91b7226b-6cce-488c-9898-747b8408a5bc"), new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8346), "00000000-0000-0000-0000-000000000006", 1, 1, new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8346), new Guid("00000000-0000-0000-0000-000000000006") });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Created", "Data", "Status", "Type", "Updated", "UserId" },
                values: new object[] { new Guid("bc70f866-70df-41c5-b21f-67d7b7634f37"), new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8349), "00000000-0000-0000-0000-000000000007", 1, 1, new DateTime(2021, 2, 12, 23, 39, 22, 965, DateTimeKind.Utc).AddTicks(8350), new Guid("00000000-0000-0000-0000-000000000007") });

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
