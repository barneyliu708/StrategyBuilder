using Microsoft.EntityFrameworkCore.Migrations;

namespace StrategyBuilder.Repository.Migrations
{
    public partial class AddActionField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_Users_CreatedById",
                table: "Strategies");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Strategies",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "JoinStrategyEventGroup",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_Users_CreatedById",
                table: "Strategies",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_Users_CreatedById",
                table: "Strategies");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "JoinStrategyEventGroup");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Strategies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_Users_CreatedById",
                table: "Strategies",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
