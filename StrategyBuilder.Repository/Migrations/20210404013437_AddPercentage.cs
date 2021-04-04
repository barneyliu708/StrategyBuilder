using Microsoft.EntityFrameworkCore.Migrations;

namespace StrategyBuilder.Repository.Migrations
{
    public partial class AddPercentage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "JoinStrategyEventGroup",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "JoinStrategyEventGroup");
        }
    }
}
