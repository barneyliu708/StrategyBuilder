using Microsoft.EntityFrameworkCore.Migrations;

namespace StrategyBuilder.Repository.Migrations
{
    public partial class AddStrategyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StrategyId",
                table: "EventGroups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Strategies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Strategies_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGroups_StrategyId",
                table: "EventGroups",
                column: "StrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_CreatedById",
                table: "Strategies",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_EventGroups_Strategies_StrategyId",
                table: "EventGroups",
                column: "StrategyId",
                principalTable: "Strategies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGroups_Strategies_StrategyId",
                table: "EventGroups");

            migrationBuilder.DropTable(
                name: "Strategies");

            migrationBuilder.DropIndex(
                name: "IX_EventGroups_StrategyId",
                table: "EventGroups");

            migrationBuilder.DropColumn(
                name: "StrategyId",
                table: "EventGroups");
        }
    }
}
