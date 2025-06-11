using budget_app.Data.Models;
using budget_app.Data.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using budget_app.Data;
using budget_app.Shared;
using budget_app.Enums;
using Microsoft.JSInterop;

namespace budget_app.Services;

public class BudgetItemService(IDbContextFactory<BudgetAppDbContext> contextFactory
, IJSRuntime JS) : IBudgetItemService
{
    private readonly IDbContextFactory<BudgetAppDbContext> _contextFactory = contextFactory;
    private readonly IJSRuntime _JS = JS;
    public async void PromptIsCompletedWhenDateArrives(int userId)
    {
        await Task.Delay(1000);

        var context = _contextFactory.CreateDbContext();

        var itemsToFlag = context.BudgetItems.Where(i => i.UserId == userId
        && i.Date <= DateTime.Now
        && i.IsCompleted == false).ToList();

        foreach (var item in itemsToFlag)
        {
            var isOk = await _JS.InvokeAsync<bool>("confirm", [$"Set budget item {item.Name} to completed?"]);

            if (isOk)
            {
                try
                {
                    item.IsCompleted = true;
                    context.BudgetItems.Update(item);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            else
            {
                try
                {
                    item.Date = DateTime.Now.AddDays(1);
                    context.BudgetItems.Update(item);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
        }
    }

}
