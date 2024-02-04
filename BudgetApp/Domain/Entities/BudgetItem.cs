using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class BudgetItem
	{
		public BudgetItem()
        {
            Name = "test name";
        }

        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Rate Rate { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
	}
}

