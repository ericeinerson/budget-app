using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace budget_app.Migrations
{
    /// <inheritdoc />
    public partial class addweeks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "BalancedTotalsTrackingLogs",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "BalancedTotalsTrackingLogs",
                newName: "YearlyTotalsBalancedBase");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentTotalsBalanced",
                table: "BalancedTotalsTrackingLogs",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyTotalsBalancedBase",
                table: "BalancedTotalsTrackingLogs",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyTotalsBalancedBase",
                table: "BalancedTotalsTrackingLogs",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeeklyTotalsBalancedBase",
                table: "BalancedTotalsTrackingLogs",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Weeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weeks", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3358));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3408));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3410));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3413));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3415));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3417));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3419));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3421));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3423));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3610));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3611));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3612));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3614));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3615));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3591));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3593));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3594));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3595));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3511));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3513));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50002,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 19, 22, 20, 1, DateTimeKind.Local).AddTicks(3509));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weeks");

            migrationBuilder.DropColumn(
                name: "CurrentTotalsBalanced",
                table: "BalancedTotalsTrackingLogs");

            migrationBuilder.DropColumn(
                name: "DailyTotalsBalancedBase",
                table: "BalancedTotalsTrackingLogs");

            migrationBuilder.DropColumn(
                name: "MonthlyTotalsBalancedBase",
                table: "BalancedTotalsTrackingLogs");

            migrationBuilder.DropColumn(
                name: "WeeklyTotalsBalancedBase",
                table: "BalancedTotalsTrackingLogs");

            migrationBuilder.RenameColumn(
                name: "YearlyTotalsBalancedBase",
                table: "BalancedTotalsTrackingLogs",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "BalancedTotalsTrackingLogs",
                newName: "Date");

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(8970));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9022));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9024));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9026));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9029));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9031));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9033));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9035));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9037));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9165));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9166));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9167));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9168));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9209));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9145));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9146));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9149));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9127));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9129));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50002,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 37, 10, 279, DateTimeKind.Local).AddTicks(9125));
        }
    }
}
