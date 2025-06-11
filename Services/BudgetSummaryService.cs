using budget_app.Data.Models;
using budget_app.Data.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using budget_app.Data;
using budget_app.Shared;
using budget_app.Enums;

namespace budget_app.Services;

public class BudgetSummaryService(IDbContextFactory<BudgetAppDbContext> contextFactory) : IBudgetSummaryService
{
    private readonly IDbContextFactory<BudgetAppDbContext> _contextFactory = contextFactory;
    private readonly DateTime _currentYearEnd = new(12, 31, DateTime.Now.Year, 23, 59, 59);
    public async Task<BudgetSummaryCompiledDetails> GetCompiledDetails(int currentUserId)
    {
        await Task.Delay(1000);

        var context = _contextFactory.CreateDbContext();

        var currentUser = context.Users.FirstOrDefault(u => u.Id == currentUserId);
        var currentBalance = currentUser?.Balance;

        var remainingIncomes = CalculateRemainingIncomes(currentUserId);
        var remainingExpenses = CalculateRemainingExpenses(currentUserId);

        var budgetSummaryCompiledDetails = new BudgetSummaryCompiledDetails()
        {
            CurrentBalance = currentBalance,
            RemainingIncomes = remainingIncomes,
            RemainingExpenses = remainingExpenses
        };

        return budgetSummaryCompiledDetails;
    }

    public decimal CalculateRemainingIncomes(int userId)
    {
        decimal incomesTotaled;
        var context = _contextFactory.CreateDbContext();

        var user = context.Users.FirstOrDefault(u => u.Id == userId);

        var allFutureIncomes = context.BudgetItems.Where(i => i.ItemTypeId == (int)Enums.ItemType.Income
        && i.UserId == userId
        && i.Date >= DateTime.Now
        && i.Date <= _currentYearEnd
        ).ToList();

        incomesTotaled = allFutureIncomes.Sum(i => i.Amount);

        return incomesTotaled;
    }

    public decimal CalculateRemainingExpenses(int userId)
    {
        decimal expensesTotaled;
        var context = _contextFactory.CreateDbContext();

        var user = context.Users.FirstOrDefault(u => u.Id == userId);

        var allExpenses = context.BudgetItems.Where(i => i.ItemTypeId == (int)Enums.ItemType.Expense
        && i.UserId == userId
        && i.Date >= DateTime.Now
        && i.Date <= _currentYearEnd
        ).ToList();

        expensesTotaled = allExpenses.Sum(i => i.Amount);

        return expensesTotaled;
    }
}
