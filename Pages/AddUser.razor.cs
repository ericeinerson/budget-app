using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Pages;

public partial class AddUser
{
    [Parameter]
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
            if(User is not null)
            {
                using var context = ContextFactory.CreateDbContext();
                
                User.CreatedDate = DateTime.Now;

                context.Users.Add(User);

                await context.SaveChangesAsync();

                SuccessMessage = $"User {User.Name} was added successfully";
                ErrorMessage = null;

                User = new()
                {
                    Name = User.Name,
                    Balance = User.Balance
                };
            }
        }
        catch (Exception ex)
        {
            SuccessMessage = null;
            ErrorMessage = $"Error while adding User: {ex.Message}.";
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