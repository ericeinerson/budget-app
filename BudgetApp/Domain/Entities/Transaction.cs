using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class Transaction
	{
		public long TransactionId { get; set; }
		public long UserAccountId { get; set; }
		public string? UpdateDate { get; set; }
		public TransactionType TransactionType { get; set; }
		public string? Description { get; set; }
		public Decimal TransactionAmount { get; set; }
		public DateTime TransactionDate { get; set; }
	}
}

