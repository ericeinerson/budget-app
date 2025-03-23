using System.ComponentModel.DataAnnotations;

namespace budget_app.Data.Models
{
    public class Adjustment
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Notes { get; set; }
    }
}
