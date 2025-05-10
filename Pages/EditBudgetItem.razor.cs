using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Pages;


public partial class EditBudgetItem
{
    [Parameter]
    public int BudgetItemId { get; set; }

    private BudgetItem? BudgetItem { get; set; }

    private Category[]? Categories { get; set; }
    private ItemType[]? ItemTypes { get; set; }

    private bool IsBusy { get; set; }

    private string? ErrorMessage { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        IsBusy = true;

        try
        {
            using var context = ContextFactory.CreateDbContext();

            Categories = await context.Categories
                            .AsNoTracking()
                            .OrderBy(c => c.Name)
                            .ToArrayAsync();
            ItemTypes = await context.ItemTypes
                            .AsNoTracking()
                            .OrderBy(i => i.Name)
                            .ToArrayAsync();
            BudgetItem = await context.BudgetItems
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.Id == BudgetItemId);
        }
        finally
        {
            IsBusy = false;
        }
    }

        private async Task HandleSubmit(bool isValid)
        {
            if(BudgetItem is null || IsBusy || !isValid)
            {
                ErrorMessage = null;
                return;
            }

            IsBusy = true;

            try
            {
                using var context = ContextFactory.CreateDbContext();
                context.Update(BudgetItem);
                await context.SaveChangesAsync();

                NavigateToSummary();
            }
            catch(DbUpdateConcurrencyException)
            {
                ErrorMessage = "The budget item was modified by another user. Please reload this page";
            }
            catch(Exception ex)
            {
                ErrorMessage = $"Error while saving budget item: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void NavigateToSummary()
        {
            Navigation.NavigateTo($"/archive/{StateContainer.BudgetItemOverviewPage}");
        }
}