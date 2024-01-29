using System;
using BudgetApp.Domain.Enums;
namespace BudgetApp.Domain.Entities
{
	public class Transaction
	{
        public Transaction()
        {
            Name = "test name";
            AmountFormatted = "$0.00";
        }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string AmountFormatted { get; set; }
        public DateTime Date { get; set; }
        public Rate Rate { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public TransactionType TransactionType { get; set; }
        public Status Status { get; set; }
    }
}

