using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace budget_app.Migrations
{
    /// <inheritdoc />
    public partial class moveadjustmenttoitemtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Balance = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BudgetItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecondaryName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetItems_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Rent" },
                    { 2, "Food" },
                    { 3, "Medical" },
                    { 4, "Miscellaneous" },
                    { 5, "Gym" }
                });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Expense" },
                    { 2, "Income" },
                    { 3, "Wishlist" },
                    { 4, "Adjustment" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Balance", "Name" },
                values: new object[,]
                {
                    { 45, 110.01m, "Sir Jaxolantern" },
                    { 49, 50000.09m, "Tacoramadan" },
                    { 50002, 10.01m, "Sir Floofsigus" }
                });

            migrationBuilder.InsertData(
                table: "BudgetItems",
                columns: new[] { "Id", "Amount", "CategoryId", "Date", "ItemTypeId", "Name", "Notes", "SecondaryName", "UserId" },
                values: new object[,]
                {
                    { 1, 0m, 2, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Expense1", "This was a test rent payment", "Rent Payment", 45 },
                    { 2, 0m, 1, new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Expense2", "Got chipotle", "Chipotle", 45 },
                    { 3, 0m, 2, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "ThirdExpense", null, "Taco Prescription Diet", 45 },
                    { 4, 0m, 3, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Expense7Rent", "This was another test rent payment", "Rent Payment", 49 },
                    { 5, 0m, 1, new DateTime(1999, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "5.0", "Got Subway", "Subs", 49 },
                    { 6, 0m, 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Expense", null, "6terst", 49 },
                    { 7, 0m, 3, new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Music", "Subscription payment", "Spotify", 49 },
                    { 8, 0m, 3, new DateTime(1954, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Caffeine", "Got chipotle", "McCoffee", 49 },
                    { 9, 0m, 4, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Nine", null, "Zelda", 50002 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_CategoryId",
                table: "BudgetItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_ItemTypeId",
                table: "BudgetItems",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_UserId",
                table: "BudgetItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
