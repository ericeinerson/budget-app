using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class BudgetItem
	{
		public BudgetItem()
        {
            Id = -1;
            CategoryId = -1;
            Name = "test name";
            Amount = 0.00M;
            StartDate = DateTime.MinValue;
            Rate = Rate.NoRate;
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Rate Rate { get; set; }
        public bool AmountVariable { get; set; }

        public void DisplayAllTransactionsForItem(UserAccount selectedAccount)
        {
            var transactionsList = selectedAccount.TransactionList.Where(t => t.BudgetItemId == this.Id);
            var postedDateForTransaction = string.Empty;

            foreach(var transaction in transactionsList)
            {
                postedDateForTransaction = transaction.PostedDate == null ? "N/A" : transaction.PostedDate.ToString();

                Console.WriteLine($"" +
                    $"Name: {transaction.Name}; " +
                    $"Amount: {transaction.Amount}; " +
                    $"Id: {transaction.Id}; " +
                    $"Category Id: {transaction.CategoryId}; " +
                    $"Scheduled Date: {transaction.ScheduledDate}; " +
                    $"Created Date: {transaction.CreatedDate}; " +
                    $"Type: {transaction.BudgetItemType}; " +
                    $"Scheduled Date: {transaction.ScheduledDate}; " +
                    $"Status: {transaction.Status}; " +
                    $"Posted Date: {postedDateForTransaction}; ");
            }
        }


    }

}

