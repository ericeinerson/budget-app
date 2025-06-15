using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace budget_app.Migrations
{
    /// <inheritdoc />
    public partial class addbalancedTotalsTrackingLogtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BalancedTotalsTrackingLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalancedTotalsTrackingLogs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalancedTotalsTrackingLogs");

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7500));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7550));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7553));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7555));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7558));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7560));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7562));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7564));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7566));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7701));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7703));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7704));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7705));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7707));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7683));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7685));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7686));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7687));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7664));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7666));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50002,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 14, 18, 20, 37, 976, DateTimeKind.Local).AddTicks(7661));
        }
    }
}
