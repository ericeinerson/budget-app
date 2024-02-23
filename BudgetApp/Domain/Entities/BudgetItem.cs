using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class BudgetItem
	{
		public BudgetItem()
        {
            Id = -1;
            CategoryId = -1;
            Name = "test name";
            Amount = 0.00M;
            StartDate = DateTime.MinValue;
            Rate = Rate.NoRate;
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Rate Rate { get; set; }
        public DateTime? MarkerDate { get; set; }
	}
}

