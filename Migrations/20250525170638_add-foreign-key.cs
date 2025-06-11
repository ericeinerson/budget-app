using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace budget_app.Migrations
{
    /// <inheritdoc />
    public partial class addforeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "BudgetItems",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "UserId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "UserId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "UserId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "UserId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "UserId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "UserId1",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_UserId1",
                table: "BudgetItems",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItems_Users_UserId1",
                table: "BudgetItems",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItems_Users_UserId1",
                table: "BudgetItems");

            migrationBuilder.DropIndex(
                name: "IX_BudgetItems_UserId1",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BudgetItems");
        }
    }
}
