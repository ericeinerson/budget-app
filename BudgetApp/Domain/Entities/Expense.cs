using System;
namespace BudgetApp.Domain.Entities
{
	public class Expense : BudgetItem
	{
        public Expense()
        {
            ExpenseId = -1;
        }
        public int ExpenseId { get; set; }

    }
}

