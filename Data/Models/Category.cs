using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace budget_app.Data.Models
{
    public class Category
    {
        public int? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public List<BudgetItem> BudgetItems { get; set; } = [];
        public DateTime CreatedDate { get; set; }

    }
}