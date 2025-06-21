using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace budget_app.Migrations
{
    /// <inheritdoc />
    public partial class takeawaynullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8142));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8192));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8194));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8196));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8198));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8200));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8202));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8204));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8206));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8335));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8336));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8338));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8339));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8340));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8317));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8318));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8320));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8321));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8298));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8300));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50002,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 21, 15, 26, 55, 460, DateTimeKind.Local).AddTicks(8296));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7068));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7123));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7125));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7131));

            migrationBuilder.UpdateData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7284));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7285));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7287));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7288));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7260));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7261));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7263));

            migrationBuilder.UpdateData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7264));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7242));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50002,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 17, 20, 8, 49, 932, DateTimeKind.Local).AddTicks(7238));
        }
    }
}
