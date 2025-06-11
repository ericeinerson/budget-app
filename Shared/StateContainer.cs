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

        public int GetCurrentUserId()
        {
            if (CurrentUserId == null)
            {
                CurrentUserId = -2;
            }
            return (int)CurrentUserId;
        }
    }
}