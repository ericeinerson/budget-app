using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Pages;


public partial class EditCategory
{
    [Parameter]
    public int CategoryId { get; set; }
    private Category? Category { get; set; }
    private bool IsBusy { get; set; }
    private string? ErrorMessage { get; set; }
    protected override async Task OnParametersSetAsync()
    {
        IsBusy = true;

        try
        {
            using var context = ContextFactory.CreateDbContext();

            Category = await context.Categories
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.Id == CategoryId);
        }
        finally
        {
            IsBusy = false;
        }
    }

        private async Task HandleSubmit(bool isValid)
        {
            if(Category is null || IsBusy || !isValid)
            {
                ErrorMessage = null;
                return;
            }

            IsBusy = true;

            try
            {
                using var context = ContextFactory.CreateDbContext();
                context.Update(Category);
                await context.SaveChangesAsync();

                NavigateToSummary();
            }
            catch(DbUpdateConcurrencyException)
            {
                ErrorMessage = "The category was modified by another user. Please reload this page";
            }
            catch(Exception ex)
            {
                ErrorMessage = $"Error while saving category: {ex.Message}";
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