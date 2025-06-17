using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using budget_app.Data.Models;

namespace budget_app.Shared
{
    public class StateContainer
    {
        public int BudgetItemOverviewPage { get; set; }
        public int? CurrentUserId { get; set; } = -2;
        public decimal BaseWeekAmount { get; set; }
        public decimal BaseMonthAmount { get; set; }
        public decimal BaseYearAmount { get; set; }
        public Week? CurrentWeek { get; set; }
        public int? CurrentMonth { get; set; }
        public int CurrentYear { get; set; }
        public decimal TotalsBalanced { get; set; }

        public int GetCurrentUserId()
        {
            CurrentUserId ??= -2;
            return (int)CurrentUserId;
        }
    }
}