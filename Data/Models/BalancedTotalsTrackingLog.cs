using System.ComponentModel.DataAnnotations;

namespace budget_app.Data.Models
{
    public class BalancedTotalsTrackingLog
    {
        public int Id { get; set; }
        public decimal? CurrentTotalsBalanced { get; set; }
        public decimal? DailyTotalsBalancedBase { get; set; }
        public decimal? WeeklyTotalsBalancedBase { get; set; }
        public decimal? MonthlyTotalsBalancedBase { get; set; }
        public decimal? YearlyTotalsBalancedBase { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

