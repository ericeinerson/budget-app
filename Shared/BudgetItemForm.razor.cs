using budget_app.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace budget_app.Shared;

public partial class BudgetItemForm
{
    [Parameter]
    public BudgetItem? BudgetItem { get; set; }
    [Parameter]

    public Category[]? Categories { get; set; }
    [Parameter]
    public bool IsBusy { get; set; }

    [Parameter]
    public bool IsEdit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }
    
    [Parameter]
    public EventCallback<bool> OnSubmit { get; set; }

    private async Task HandleValidSubmit()
    {
        if(OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(true);
        }
    }

    private async Task HandleInvalidSubmit()
    {
       if(OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(false);
        }
    }

    private async Task HandleCancel()
    {
        if(OnCancel.HasDelegate)
        {
            await OnCancel.InvokeAsync();
        }
    }
}