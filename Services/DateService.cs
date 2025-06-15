using budget_app.Data.Models;
using budget_app.Data.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using budget_app.Data;
using budget_app.Shared;
using budget_app.Enums;
using Microsoft.JSInterop;

namespace budget_app.Services;

public class DateService(IDbContextFactory<BudgetAppDbContext> contextFactory
, IJSRuntime JS) : IDateService
{
    private readonly IDbContextFactory<BudgetAppDbContext> _contextFactory = contextFactory;
    private readonly IJSRuntime _JS = JS;
    public async Task WriteCurrentYearWeeksToDataBase()
    {
        var context = _contextFactory.CreateDbContext();
        var startDate = new DateTime(DateTime.Now.Year, 1, 1);
        var yearEnd = startDate.AddYears(1).AddSeconds(-1);
        var endDate = startDate.AddDays(7).AddSeconds(-1);
        var daysToAdd = 7;

        if (!context.Weeks.Where(w => w.DateStart <= yearEnd && w.DateStart >= startDate).Any())
        {
            while (startDate < yearEnd)
            {
                context.Weeks.Add(new Week()
                {
                    DateStart = startDate,
                    DateEnd = endDate
                });
                startDate = startDate.AddDays(daysToAdd);
                endDate = endDate.AddDays(daysToAdd);
            }
        }

        await context.SaveChangesAsync();
    }

}
