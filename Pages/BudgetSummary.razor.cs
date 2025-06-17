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

        protected override void OnInitialized()
        {
            ConstructBudgetSummaryCompiledDetails();
            BudgetSummaryCompiledDetails ??= new BudgetSummaryCompiledDetails();
            
            StateContainer.TotalsBalanced = BudgetSummaryCompiledDetails.TotalsBalanced;
            var currentUserId = StateContainer.GetCurrentUserId();

            BudgetItemService.PromptIsCompletedWhenDateArrives(currentUserId);
        }
        public BudgetSummaryCompiledDetails ConstructBudgetSummaryCompiledDetails()
        {
            var currentUserId = StateContainer.GetCurrentUserId();
            BudgetSummaryCompiledDetails = BudgetSummaryService.GetCompiledDetails(currentUserId);
            return BudgetSummaryCompiledDetails;
        }
    }
