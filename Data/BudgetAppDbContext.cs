using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget_app.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace budget_app.Data
{
    public class BudgetAppDbContext : DbContext
    {
        public BudgetAppDbContext(DbContextOptions<BudgetAppDbContext> options) : base(options) {}
        public DbSet<BudgetItem> BudgetItems => Set<BudgetItem>();
        public DbSet<ItemType> ItemTypes => Set<ItemType>();
        public DbSet<Budget> Budgets => Set<Budget>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Adjustment> Adjustments => Set<Adjustment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BudgetItem>().HasData(
                new BudgetItem 
                {
                    Id = 1,
                    Name = "Expense1",
                    SecondaryName = "Rent Payment",
                    Notes = "This was a test rent payment",
                    ItemTypeId = 1,
                    Date = new DateTime(2025, 3, 21),
                    CategoryId = 2
                },
                new BudgetItem 
                {
                    Id = 2,
                    Name = "Expense2",
                    SecondaryName = "Chipotle",
                    Notes = "Got chipotle",
                    ItemTypeId = 2,
                    Date = new DateTime(2024, 2, 21),
                    CategoryId = 1
                },
                new BudgetItem 
                {
                    Id = 3,
                    Name = "ThirdExpense",
                    SecondaryName = "Taco Prescription Diet",
                    ItemTypeId = 2,
                    Date = new DateTime(2025, 4, 1),
                    CategoryId = 2
                }
            );
        }
    }
}