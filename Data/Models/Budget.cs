using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace budget_app.Data.Models
{
    public class Budget
    {
        public int Id { get; set; }
        [Required]
        public List<BudgetItem>? BudgetItems { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BaseBalance { get; set; }
        public decimal GoalBalance { get; set; }
        public List<Adjustment>? Adjustments { get; set; }

    }
}