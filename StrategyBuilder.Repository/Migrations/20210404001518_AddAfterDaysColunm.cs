using Microsoft.EntityFrameworkCore.Migrations;

namespace StrategyBuilder.Repository.Migrations
{
    public partial class AddAfterDaysColunm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AfterDays",
                table: "JoinStrategyEventGroup",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfterDays",
                table: "JoinStrategyEventGroup");
        }
    }
}
