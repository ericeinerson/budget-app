using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Pages;

public partial class UserDetailsInput
{
    [Parameter]
    public int UserId { get; set; }
    public User? User { get; set; }
    private BudgetItem[]? BudgetItems { get; set; }
    private bool IsBusy { get; set; }
    private string? SuccessMessage { get; set; }
    private string? ErrorMessage { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        using var context = ContextFactory.CreateDbContext();

        BudgetItems = await context.BudgetItems
        .OrderBy(c => c.Name)
        .ToArrayAsync();

        User = new()
        {
            BudgetItems = [.. BudgetItems],
            Name = string.Empty,
            Balance = 0.00M
        };
    }

    private async Task HandleSubmit(bool isValid)
    {
        if(isValid)
        {
            await HandleValidSubmit();
        }
        else
        {
            HandleInvalidSubmit();
        }
    }

    private async Task HandleValidSubmit()
    {
        if(IsBusy)
        {
            return;
        }

        IsBusy = true;

        try
        {
            if(BudgetItem is not null)
            {
                using var context = ContextFactory.CreateDbContext();

                context.BudgetItems.Add(BudgetItem);

                await context.SaveChangesAsync();

                SuccessMessage = $"Item {BudgetItem.Name} was added successfully";
                ErrorMessage = null;

                BudgetItem = new()
                {
                    CategoryId = BudgetItem.CategoryId,
                    ItemTypeId = BudgetItem.ItemTypeId,
                    Date = DateTime.Now,
                    Amount = 0.00M
                };
            }
        }
        catch (Exception ex)
        {
            SuccessMessage = null;
            ErrorMessage = $"Error while adding Item: {ex.Message}.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void HandleInvalidSubmit()
    {
        SuccessMessage = null;
        ErrorMessage = null;
    }
}