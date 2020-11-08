using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StrategyBuilder.Repository.Migrations
{
    public partial class AddBackTestingResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackTestingResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutedOn = table.Column<DateTime>(nullable: false),
                    StartFrom = table.Column<DateTime>(nullable: false),
                    EndTo = table.Column<DateTime>(nullable: false),
                    ReportFileUri = table.Column<string>(nullable: false),
                    ExecutedById = table.Column<int>(nullable: false),
                    StrategyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackTestingResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackTestingResults_Users_ExecutedById",
                        column: x => x.ExecutedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackTestingResults_Strategies_StrategyId",
                        column: x => x.StrategyId,
                        principalTable: "Strategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackTestingResults_ExecutedById",
                table: "BackTestingResults",
                column: "ExecutedById");

            migrationBuilder.CreateIndex(
                name: "IX_BackTestingResults_StrategyId",
                table: "BackTestingResults",
                column: "StrategyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackTestingResults");
        }
    }
}
