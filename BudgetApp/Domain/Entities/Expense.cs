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

        public decimal CalculateTotalExpensesForTimeRange(UserAccount selectedAccount, DateTime? startDate = null, DateTime? endDate = null)
        {
            if(startDate == null)
            {
                startDate = DateTime.Now;
            }
            else
            {
                startDate = (DateTime)startDate;
            }
            if(endDate == null)
            {
                startDate = DateTime.Now;
            }
            else
            {
                endDate = (DateTime)endDate;
            }

            if(startDate > endDate)
            {
                throw new Exception("Start Date must be after End Date");
            }

            var totalExpensesValue = 0.00M;

            var transactionsList = selectedAccount.TransactionList.Where(e => e.ScheduledDate >= startDate && e.ScheduledDate <= endDate);

            foreach (Transaction transaction in transactionsList)
            {
                totalExpensesValue += transaction.Amount;
            }
            
            return totalExpensesValue;
        }
    }
}

