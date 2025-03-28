namespace budget_app.Pages;

public partial class Archive
{
    private BudgetItem[]? BudgetItems { get; set; }

    protected override async Task OnInitializedAsync()
    {
        using var context = ContextFactory.CreateDbContext();

        BudgetItems = await context.BudgetItems.ToArrayAsync();
    }
}
