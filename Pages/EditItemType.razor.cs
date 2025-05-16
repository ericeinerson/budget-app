using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Pages;


public partial class EditItemType
{
    [Parameter]
    public int ItemTypeId { get; set; }
    private ItemType? ItemType { get; set; }
    private bool IsBusy { get; set; }
    private string? ErrorMessage { get; set; }
    protected override async Task OnParametersSetAsync()
    {
        IsBusy = true;

        try
        {
            using var context = ContextFactory.CreateDbContext();

            ItemType = await context.ItemTypes
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.Id == ItemTypeId);
        }
        finally
        {
            IsBusy = false;
        }
    }

        private async Task HandleSubmit(bool isValid)
        {
            if(ItemType is null || IsBusy || !isValid)
            {
                ErrorMessage = null;
                return;
            }

            IsBusy = true;

            try
            {
                using var context = ContextFactory.CreateDbContext();
                context.Update(ItemType);
                await context.SaveChangesAsync();

                NavigateToSummary();
            }
            catch(DbUpdateConcurrencyException)
            {
                ErrorMessage = "The item type was modified by another user. Please reload this page";
            }
            catch(Exception ex)
            {
                ErrorMessage = $"Error while saving item type: {ex.Message}";
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