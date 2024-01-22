using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class Expense
	{
        public Expense()
        {
            ExpenseName = "test name";
            AmountFormatted = "$0.00";
        }
        public int Id { get; set; }
        public string ExpenseName { get; set; }
        public decimal Amount { get; set; }
        public string AmountFormatted { get; set; }
        public Rate Rate { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
    }
}

