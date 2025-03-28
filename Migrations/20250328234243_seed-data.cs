using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace budget_app.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BudgetItems",
                columns: new[] { "Id", "Amount", "BudgetId", "CategoryId", "Date", "ItemTypeId", "Name", "Notes", "SecondaryName" },
                values: new object[,]
                {
                    { 1, 0m, null, 2, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Expense1", "This was a test rent payment", "Rent Payment" },
                    { 2, 0m, null, 1, new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Expense2", "Got chipotle", "Chipotle" },
                    { 3, 0m, null, 2, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "ThirdExpense", null, "Taco Prescription Diet" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BudgetItems",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
