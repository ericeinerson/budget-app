using System;
using BudgetApp.Domain.Enums;
namespace BudgetApp.Domain.Entities
{
	public class Income
	{
        public Income()
        {
            IncomeName = "test name";
        }
        public string IncomeName { get; set; }
        public decimal Amount { get; set; }
        public Rate Rate { get; set; }
        public int Id { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}

