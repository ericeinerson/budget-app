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
    public async Task<BudgetSummaryCompiledDetails> GetCompiledDetails(int currentUserId)
    {
        var context = await _contextFactory.CreateDbContextAsync();

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

    public async Task<BudgetSummaryTotalsTracking> GetTrackedTotals(int currentUserId, BudgetSummaryCompiledDetails budgetCompiledDetails)
    {
        var context = await _contextFactory.CreateDbContextAsync();

        var currentUser = context.Users.FirstOrDefault(u => u.Id == currentUserId);

        var year = DateTime.Now.Year;
        var month = DateTime.Now.Month;
        var week = context.Weeks.First(w => w.DateStart <= DateTime.Now && w.DateEnd >= DateTime.Now);

        var yearStartTotalsBalanced = context.BalancedTotalsTrackingLogs.Where(b => b.CreatedDate.Year == year && b.UserId == currentUserId).ToList().Select(t => t.CurrentTotalsBalanced).Min();
        var yearCurrentTotalsBalanced = context.BalancedTotalsTrackingLogs.Where(b => b.CreatedDate.Year == year && b.UserId == currentUserId).ToList().Select(t => t.CurrentTotalsBalanced).Max();

        var monthStartTotalsBalanced = context.BalancedTotalsTrackingLogs.Where(b => b.CreatedDate.Month == month && b.UserId == currentUserId).ToList().Select(t => t.CurrentTotalsBalanced).Min();
        var monthCurrentTotalsBalanced = context.BalancedTotalsTrackingLogs.Where(b => b.CreatedDate.Month == month && b.UserId == currentUserId).ToList().Select(t => t.CurrentTotalsBalanced).Max();

        var weekStartTotalsBalanced = context.BalancedTotalsTrackingLogs.Where(b => b.CreatedDate >= week.DateStart && b.CreatedDate <= week.DateEnd && b.UserId == currentUserId).ToList().Select(t => t.CurrentTotalsBalanced);
        var weekStartTotalsBalancedMin = 0.00M;
        var weekCurrentTotalsBalancedMax = 0.00M;

        if (weekStartTotalsBalanced.Any())
        {
            weekStartTotalsBalancedMin = weekStartTotalsBalanced.Min();
        }
        // weekStartTotalsBalanced = context.BalancedTotalsTrackingLogs.Where(b => b.CreatedDate <= week.DateStart && b.UserId == currentUserId).ToList().Select(t => t.CurrentTotalsBalanced).Max();
        if (weekStartTotalsBalanced.Any())
        {
            weekCurrentTotalsBalancedMax = context.BalancedTotalsTrackingLogs.Where(b => b.CreatedDate >= week.DateStart && b.CreatedDate <= week.DateEnd && b.UserId == currentUserId).ToList().Select(t => t.CurrentTotalsBalanced).Max();
        }
        // weekCurrentTotalsBalanced = weekStartTotalsBalanced;
        

        var budgetSummaryTotalsTracked = new BudgetSummaryTotalsTracking()
        {
            TotalsBalanced = budgetCompiledDetails.TotalsBalanced,
            CurrentYearDifference = yearStartTotalsBalanced - budgetCompiledDetails.TotalsBalanced,
            CurrentMonthDifference = monthStartTotalsBalanced - budgetCompiledDetails.TotalsBalanced,
            CurrentWeekDifference = weekStartTotalsBalancedMin - budgetCompiledDetails.TotalsBalanced,
            CurrentDayDifference = 0,
            JanuaryDifference = CalculateMonthDifferences(currentUserId, 1),
            FebruarayDifference = CalculateMonthDifferences(currentUserId, 2),
            MarchDifference = CalculateMonthDifferences(currentUserId, 3),
            AprilDifference = CalculateMonthDifferences(currentUserId, 4),
            MayDifference = CalculateMonthDifferences(currentUserId, 5),
            JuneDifference = CalculateMonthDifferences(currentUserId, 6),
            JulyDifference = CalculateMonthDifferences(currentUserId, 7),
            AugustDifference = CalculateMonthDifferences(currentUserId, 8),
            SeptemberDifference = CalculateMonthDifferences(currentUserId, 9),
            OctoberDifference = CalculateMonthDifferences(currentUserId, 10),
            NovemberDifference = CalculateMonthDifferences(currentUserId, 11),
            DecemberDifference = CalculateMonthDifferences(currentUserId, 12)
        };

        return budgetSummaryTotalsTracked;
    }

    public decimal CalculateMonthDifferences(int currentUserId, int month)
    {
        var context = _contextFactory.CreateDbContext();

        if (context.BalancedTotalsTrackingLogs.Where(b =>
            b.CreatedDate.Year == DateTime.Now.Year
            && b.CreatedDate.Month == month
            && b.UserId == currentUserId).Any())
        {
            var monthStartTotalsBalanced = context.BalancedTotalsTrackingLogs.Where(b =>
            b.CreatedDate.Year == DateTime.Now.Year
            && b.CreatedDate.Month == month
            && b.UserId == currentUserId).ToList().Select(t => t.MonthlyTotalsBalancedBase).Min();

            var lastOrCurrentMonthTrackingLogDate = context.BalancedTotalsTrackingLogs.Where(b =>
            b.CreatedDate.Year == DateTime.Now.Year
            && b.CreatedDate.Month == month
            && b.UserId == currentUserId).ToList().Select(t => t.CreatedDate).Max();

            var monthEndOrCurrentTotalsBalanced = context.BalancedTotalsTrackingLogs.First(b =>
            b.CreatedDate == lastOrCurrentMonthTrackingLogDate).CurrentTotalsBalanced;

            var difference = monthStartTotalsBalanced - monthEndOrCurrentTotalsBalanced;

            return difference;
        }
        else
        {
            return 0.00M;
        }
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

    public void EnterBalancedTotalsTrackingLog(BudgetSummaryCompiledDetails compiledDetails, int userId)
    {
        var context = _contextFactory.CreateDbContext();
        var totalsBalanced = compiledDetails.TotalsBalanced;
        var lastTrackingLog = new BalancedTotalsTrackingLog();
        var currentWeek = context.Weeks.First(w => w.DateStart <= DateTime.Now && w.DateEnd >= DateTime.Now);
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;
        var balancedTotalsTrackingLog = new BalancedTotalsTrackingLog()
        {
            CurrentTotalsBalanced = totalsBalanced,
            DailyTotalsBalancedBase = totalsBalanced,
            WeeklyTotalsBalancedBase = totalsBalanced,
            MonthlyTotalsBalancedBase = totalsBalanced,
            YearlyTotalsBalancedBase = totalsBalanced,
            CreatedDate = DateTime.Now,
            UserId = userId
        };

        if (context.BalancedTotalsTrackingLogs.Where(b => b.UserId == userId).Any())
        {
            var lastTrackingLogDate = context.BalancedTotalsTrackingLogs.ToList().Select(t => t.CreatedDate).Max();

            lastTrackingLog = context.BalancedTotalsTrackingLogs.First(t => t.CreatedDate == lastTrackingLogDate);

            if (lastTrackingLog is not null)
            {
                if (lastTrackingLog.CreatedDate <= currentWeek.DateEnd)
                {
                    balancedTotalsTrackingLog.WeeklyTotalsBalancedBase = lastTrackingLog.WeeklyTotalsBalancedBase;
                }

                if (lastTrackingLog.CreatedDate.Month == currentMonth && lastTrackingLog.CreatedDate.Year == currentYear)
                {
                    balancedTotalsTrackingLog.MonthlyTotalsBalancedBase = lastTrackingLog.MonthlyTotalsBalancedBase;
                }

                if (lastTrackingLog.CreatedDate.Year == currentYear)
                {
                    balancedTotalsTrackingLog.YearlyTotalsBalancedBase = lastTrackingLog.YearlyTotalsBalancedBase;
                }
            }
        }

        context.BalancedTotalsTrackingLogs.Add(balancedTotalsTrackingLog);
        context.SaveChanges();
    }
}
