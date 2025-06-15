using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Pages;


public partial class Archive
{
    private BudgetItem[]? BudgetItems { get; set; }
    private int TotalPages { get; set; }
    [Parameter]
    public int? CurrentPage { get; set; }
    public const int NumberOfItemsToShow = 6;
    protected override async Task OnParametersSetAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        if(CurrentPage is null or < 1)
        {
            Navigation.NavigateTo("/archive/1");
            return;
        }

        using var context = ContextFactory.CreateDbContext();
        var currentUserId = StateContainer.GetCurrentUserId();
        var itemCount = await context.BudgetItems.Where(b => b.UserId == currentUserId).CountAsync();
        TotalPages = itemCount == 0 
            ? 1 
            : (int)Math.Ceiling((double)itemCount / NumberOfItemsToShow);

        if(CurrentPage > TotalPages)
        {
            Navigation.NavigateTo($"/archive/{TotalPages}");
            return;
        }

        StateContainer.BudgetItemOverviewPage = CurrentPage.Value;

        var itemsToSkip = (CurrentPage.Value - 1) * NumberOfItemsToShow;

        BudgetItems = await context.BudgetItems
            .Where(b => b.UserId == currentUserId)
            .Include(b => b.Category)
            .Include(b => b.ItemType)
            .OrderByDescending(b => b.Date)
            .Skip(itemsToSkip)
            .Take(NumberOfItemsToShow)
            .ToArrayAsync();
    }

    private async Task HandleDelete(BudgetItem budgetItem)
    {
        var isOk = await JS.InvokeAsync<bool>("confirm", [$"Delete budget item {budgetItem.Name}?"]);

        if(isOk)
        {
            try
            {
                using var context = ContextFactory.CreateDbContext();
                context.BudgetItems.Remove(budgetItem);
                await context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {

            }

            await LoadData();
        }
    }
}
