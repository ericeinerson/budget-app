using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Pages;


public partial class EditUser
{
    [Parameter]
    public int UserId { get; set; }
    private User? User { get; set; }
    private BudgetItem[]? BudgetItems { get; set; }
    private bool IsBusy { get; set; }
    private string? ErrorMessage { get; set; }
    protected override async Task OnParametersSetAsync()
    {
        IsBusy = true;

        try
        {
            using var context = ContextFactory.CreateDbContext();

            BudgetItems = await context.BudgetItems
                            .AsNoTracking()
                            .OrderBy(c => c.Name)
                            .ToArrayAsync();
            User = await context.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.Id == UserId);
        }
        finally
        {
            IsBusy = false;
        }
    }

        private async Task HandleSubmit(bool isValid)
        {
            if(User is null || IsBusy || !isValid)
            {
                ErrorMessage = null;
                return;
            }

            IsBusy = true;

            try
            {
                using var context = ContextFactory.CreateDbContext();
                context.Update(User);
                await context.SaveChangesAsync();

                NavigateToSummary();
            }
            catch(DbUpdateConcurrencyException)
            {
                ErrorMessage = "The usser was modified by another user. Please reload this page";
            }
            catch(Exception ex)
            {
                ErrorMessage = $"Error while saving user: {ex.Message}";
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