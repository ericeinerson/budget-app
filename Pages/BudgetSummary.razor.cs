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

        public BudgetSummaryCompiledDetails? BudgetSummaryCompiledDetails { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ConstructBudgetSummaryCompiledDetails();
            var currentUserId = StateContainer.GetCurrentUserId();

            BudgetItemService.PromptIsCompletedWhenDateArrives(currentUserId);
        }
        public async Task<BudgetSummaryCompiledDetails> ConstructBudgetSummaryCompiledDetails()
        {
            var currentUserId = StateContainer.GetCurrentUserId();
            BudgetSummaryCompiledDetails = await BudgetSummaryService.GetCompiledDetails(currentUserId);
            return BudgetSummaryCompiledDetails;
        }
    }
