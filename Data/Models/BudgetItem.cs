using System.ComponentModel.DataAnnotations;

namespace budget_app.Data.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? SecondaryName { get; set; }
        public string? Notes { get; set; }
        public int ItemTypeId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}

