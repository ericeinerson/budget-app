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
        [Required]
        public int? ItemTypeId { get; set; }
        public ItemType? ItemType { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int? CategoryId { get; set; }

        public Category? Category { get; set; }
        [Required]
        public int? UserId { get; set; }
        public User? User { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        [Timestamp]
        public byte[]? Timestamp { get; set; }
    }
}

