using budget_app.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Pages;

public partial class AddBudgetItem
{
    public BudgetItem? BudgetItem { get; set; }

    public Category[]? Categories { get; set; }

    private bool IsBusy { get; set; }

    private string? SuccessMessage { get; set; }

    private string? ErrorMessage { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        using var context = ContextFactory.CreateDbContext();

        Categories = await context.Categories
        .OrderBy(c => c.Name)
        .ToArrayAsync();


        BudgetItem = new()
        {
            CategoryId = Categories.FirstOrDefault()?.Id
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
                    CategoryId = BudgetItem.CategoryId
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