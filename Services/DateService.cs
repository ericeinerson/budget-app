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
    public void WriteCurrentYearWeeksToDataBase()
    {
        var context = _contextFactory.CreateDbContext();
    }

}
