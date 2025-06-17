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
    private readonly DateTime _currentYearEnd = new(DateTime.Now.Year, 12, 31, 23, 59, 59);
    public BudgetSummaryCompiledDetails GetCompiledDetails(int currentUserId)
    {
        var context = _contextFactory.CreateDbContext();

        var currentUser = context.Users.FirstOrDefault(u => u.Id == currentUserId);
        var currentBalance = currentUser == null ? 0.00M : currentUser.Balance;

        var remainingIncomes = CalculateRemainingIncomes(currentUserId);
        var remainingExpenses = CalculateRemainingExpenses(currentUserId);
        var futureAdjustmentsNegative = CalculateFutureAdjustmentsNegative(currentUserId) * -1;
        var futureAdjustmentsPositive = CalculateFutureAdjustmentsPositive(currentUserId);
        var totalsBalanced = currentBalance + remainingIncomes - remainingExpenses + futureAdjustmentsPositive - futureAdjustmentsNegative;

        var budgetSummaryCompiledDetails = new BudgetSummaryCompiledDetails()
        {
            CurrentBalance = currentBalance,
            RemainingIncomes = remainingIncomes,
            RemainingExpenses = remainingExpenses,
            AdjustmentsNegative = futureAdjustmentsNegative,
            AdjustmentsPositive = futureAdjustmentsPositive,
            TotalsBalanced = totalsBalanced
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

    public decimal CalculateFutureAdjustmentsNegative(int userId)
    {
        decimal futureAdjustmentsNegative;
        var context = _contextFactory.CreateDbContext();

        var user = context.Users.FirstOrDefault(u => u.Id == userId);

        var adjustments = context.BudgetItems.Where(i => i.ItemTypeId == (int)Enums.ItemType.Adjustment
        && i.UserId == userId
        && i.Date.Year == DateTime.Now.Year
        && i.Date >= DateTime.Now
        && i.Amount < 0
        ).ToList();

        futureAdjustmentsNegative = adjustments.Sum(i => i.Amount);

        return futureAdjustmentsNegative;
    }

    public decimal CalculateFutureAdjustmentsPositive(int userId)
    {
        decimal futureAdjustmentsNegative;
        var context = _contextFactory.CreateDbContext();

        var user = context.Users.FirstOrDefault(u => u.Id == userId);

        var adjustments = context.BudgetItems.Where(i => i.ItemTypeId == (int)Enums.ItemType.Adjustment
        && i.UserId == userId
        && i.Date.Year == DateTime.Now.Year
        && i.Date >= DateTime.Now
        && i.Amount >= 0
        ).ToList();

        futureAdjustmentsNegative = adjustments.Sum(i => i.Amount);

        return futureAdjustmentsNegative;
    }
    
    public decimal CalculateTotalsBalanced(int userId)
    {
        decimal futureAdjustmentsNegative;
        var context = _contextFactory.CreateDbContext();

        var user = context.Users.FirstOrDefault(u => u.Id == userId);

        var adjustments = context.BudgetItems.Where(i => i.ItemTypeId == (int)Enums.ItemType.Adjustment
        && i.UserId == userId
        && i.Date.Year == DateTime.Now.Year
        && i.Date >= DateTime.Now
        && i.Amount >= 0
        ).ToList();

        futureAdjustmentsNegative = adjustments.Sum(i => i.Amount);

        return futureAdjustmentsNegative;
    }
}
