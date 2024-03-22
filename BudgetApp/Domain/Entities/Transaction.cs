using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class Transaction
	{
        public Transaction()
        {
            Id = -1;
            CategoryId = 0;
            BudgetItemId = -1;
            Name = "test name";
            Amount = 0.00M;
            CreatedDate = DateTime.MinValue;
            PostedDate = DateTime.MinValue;
            BudgetItemType = BudgetItemType.All;
            Status = Status.Pending;
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int BudgetItemId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? PostedDate { get; set; }
        public BudgetItemType BudgetItemType { get; set; }
        public Status Status { get; set; }
    }
}

