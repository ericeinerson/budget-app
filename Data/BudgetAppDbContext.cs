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
        public DbSet<User> Users => Set<User>();
        public DbSet<ItemType> ItemTypes => Set<ItemType>();
        public DbSet<Category> Categories => Set<Category>();

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
                    CategoryId = 2,
                    UserId = 45
                },
                new BudgetItem 
                {
                    Id = 2,
                    Name = "Expense2",
                    SecondaryName = "Chipotle",
                    Notes = "Got chipotle",
                    ItemTypeId = 2,
                    Date = new DateTime(2024, 2, 21),
                    CategoryId = 1,
                    UserId = 45
                },
                new BudgetItem 
                {
                    Id = 3,
                    Name = "ThirdExpense",
                    SecondaryName = "Taco Prescription Diet",
                    ItemTypeId = 2,
                    Date = new DateTime(2025, 4, 1),
                    CategoryId = 2,
                    UserId = 45
                },
                new BudgetItem 
                {
                    Id = 4,
                    Name = "Expense7Rent",
                    SecondaryName = "Rent Payment",
                    Notes = "This was another test rent payment",
                    ItemTypeId = 1,
                    Date = new DateTime(2000, 1, 1),
                    CategoryId = 3,
                    UserId = 49
                },
                new BudgetItem 
                {
                    Id = 5,
                    Name = "5.0",
                    SecondaryName = "Subs",
                    Notes = "Got Subway",
                    ItemTypeId = 3,
                    Date = new DateTime(1999, 12, 31),
                    CategoryId = 1,
                    UserId = 49
                },
                new BudgetItem 
                {
                    Id = 6,
                    Name = "Expense",
                    SecondaryName = "6terst",
                    ItemTypeId = 1,
                    Date = new DateTime(2025, 10, 1),
                    CategoryId = 1,
                    UserId = 49
                },
                new BudgetItem 
                {
                    Id = 7,
                    Name = "Music",
                    SecondaryName = "Spotify",
                    Notes = "Subscription payment",
                    ItemTypeId = 1,
                    Date = new DateTime(2025, 3, 31),
                    CategoryId = 3,
                    UserId = 49
                },
                new BudgetItem 
                {
                    Id = 8,
                    Name = "Caffeine",
                    SecondaryName = "McCoffee",
                    Notes = "Got chipotle",
                    ItemTypeId = 2,
                    Date = new DateTime(1954, 12, 21),
                    CategoryId = 3,
                    UserId = 49
                },
                new BudgetItem 
                {
                    Id = 9,
                    Name = "Nine",
                    SecondaryName = "Zelda",
                    ItemTypeId = 3,
                    Date = new DateTime(2025, 4, 1),
                    CategoryId = 4,
                    UserId = 50002
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User 
                {
                    Id = 50002,
                    Name = "Sir Floofsigus",
                    Balance = 10.01M
                },
                new User 
                {
                    Id = 45,
                    Name = "Sir Jaxolantern",
                    Balance = 110.01M
                },
                new User 
                {
                    Id = 49,
                    Name = "Tacoramadan",
                    Balance = 50000.09M
                }
            );

            modelBuilder.Entity<ItemType>().HasData(
                new ItemType 
                {
                    Id = 1,
                    Name = "Expense"
                },
                new ItemType 
                {
                    Id = 2,
                    Name = "Income"
                },
                new ItemType 
                {
                    Id = 3,
                    Name = "Wishlist"
                },
                new ItemType 
                {
                    Id = 4,
                    Name = "Adjustment"
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category 
                {
                    Id = 1,
                    Name = "Rent"
                },
                new Category 
                {
                    Id = 2,
                    Name = "Food"
                },
                new Category 
                {
                    Id = 3,
                    Name = "Medical"
                },
                new Category 
                {
                    Id = 4,
                    Name = "Miscellaneous"
                },
                new Category 
                {
                    Id = 5,
                    Name = "Gym"
                }
            );
        }
    }
}