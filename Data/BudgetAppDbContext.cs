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
    }
}