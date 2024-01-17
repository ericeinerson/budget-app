using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class Expense
	{
        public Expense()
        {
            ExpenseName = "test name";
        }
        public int Id { get; set; }
        public string ExpenseName { get; set; }
        public decimal Amount { get; set; }
        public string AmountFormatted { get; set; }
        public Rate Rate { get; set; }
        public DateTime Date { get; set; }
    }
}

