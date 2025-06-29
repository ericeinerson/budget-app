using budget_app.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget_app.Data.Models;
using budget_app.Data.DataTransferObjects;

namespace budget_app.Pages;

public partial class BudgetSummary
{

    public BudgetSummaryCompiledDetails BudgetSummaryCompiledDetails { get; set; } = new();
    public BudgetSummaryTotalsTracking BudgetSummaryTotalsTracking { get; set; } = new();

    protected async override Task OnInitializedAsync()
    {
        var currentUserId = StateContainer.GetCurrentUserId();

        await ConstructBudgetSummaryCompiledDetails();

        StateContainer.TotalsBalanced = BudgetSummaryCompiledDetails.TotalsBalanced;

        if (BudgetSummaryCompiledDetails is not null)
        {
            WriteBalancedTotalsTrackingLog(BudgetSummaryCompiledDetails, currentUserId);
            await ConstructBudgetSummaryTotalsTracking();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var currentUserId = StateContainer.GetCurrentUserId();

            await BudgetItemService.PromptIsCompletedWhenDateArrives(currentUserId);
        }
    }

    public async Task<BudgetSummaryCompiledDetails> ConstructBudgetSummaryCompiledDetails()
    {
        var currentUserId = StateContainer.GetCurrentUserId();

        BudgetSummaryCompiledDetails = await BudgetSummaryService.GetCompiledDetails(currentUserId);
        return BudgetSummaryCompiledDetails;
    }

    public async Task<BudgetSummaryTotalsTracking> ConstructBudgetSummaryTotalsTracking()
    {
        var currentUserId = StateContainer.GetCurrentUserId();

        if (BudgetSummaryCompiledDetails == null)
        {
            return new BudgetSummaryTotalsTracking();
        }

        BudgetSummaryTotalsTracking = await BudgetSummaryService.GetTrackedTotals(currentUserId, BudgetSummaryCompiledDetails);
        return BudgetSummaryTotalsTracking;
    }

    public void WriteBalancedTotalsTrackingLog(BudgetSummaryCompiledDetails compiledDetails, int userId)
    {
        BudgetSummaryService.EnterBalancedTotalsTrackingLog(compiledDetails, userId);
    }
}
