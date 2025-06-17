namespace budget_app.Data.DataTransferObjects
{
    public class BudgetSummaryCompiledDetails
    {
        public decimal CurrentBalance { get; set; }
        public decimal RemainingIncomes { get; set; }
        public decimal RemainingExpenses { get; set; }
        public decimal AdjustmentsPositive { get; set; }
        public decimal AdjustmentsNegative { get; set; }
        public decimal TotalsBalanced { get; set; }
    }
}