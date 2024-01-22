using System;
using BudgetApp.Domain.Enums;
namespace BudgetApp.Domain.Entities
{
	public class Income
	{
        public Income()
        {
            IncomeName = "test name";
            AmountFormatted = "$0.00";
        }
        public string IncomeName { get; set; }
        public decimal Amount { get; set; }
        public string AmountFormatted { get; set; }
        public DateTime Date { get; set; }
        public Rate Rate { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
    }
}

