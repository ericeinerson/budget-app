namespace budget_app.Data.DataTransferObjects
{
    public class BudgetSummaryTotalsTracking
    {
        public decimal JanuaryDifference { get; set; }
        public decimal FebruarayDifference { get; set; }
        public decimal MarchDifference { get; set; }
        public decimal AprilDifference { get; set; }
        public decimal MayDifference { get; set; }
        public decimal JuneDifference { get; set; }
        public decimal JulyDifference { get; set; }
        public decimal AugustDifference { get; set; }
        public decimal SeptemberDifference { get; set; }
        public decimal OctoberDifference { get; set; }
        public decimal NovemberDifference { get; set; }
        public decimal DecemberDifference { get; set; }
        public decimal CurrentYearDifference { get; set; }
        public decimal CurrentMonthDifference { get; set; }
        public decimal CurrentWeekDifference { get; set; }
        public decimal CurrentDayDifference { get; set; }
        public decimal TotalsBalanced { get; set; }
    }
}