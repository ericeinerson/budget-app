using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class UserAccount
	{
		public UserAccount()
		{
			Id = 1;
			Directory = "";
			Passcode = "0000";
			FullName = "test name";
			Balance = 0;
			TotalLogin = 0;
			IsLocked = true;
			IncomeId = 0;
			ExpenseId = 0;
			WishlistId = 0;
			Wishlist = new Wishlist();
			IncomeList = new List<Income>();
			ExpenseList = new List<Expense>();
			TransactionCategoryList = new List<TransactionCategory>();
		}
		public int Id { get; set; }
		public string Directory { get; set; }
		public string Passcode { get; set; }
		public string FullName { get; set; }
		public decimal Balance { get; set; }
		public int TotalLogin { get; set; }
		public bool IsLocked { get; set; }
		public decimal TotalExpenses { get; set; }
		public decimal TotalIncomes { get; set; }
		public int IncomeId { get; set; }
		public int ExpenseId { get; set; }
		public int WishlistId { get; set; }
		public Wishlist Wishlist {get;set;}
		public List<Income> IncomeList { get; set; }
        public List<Expense> ExpenseList { get; set; }
		public List<TransactionCategory> TransactionCategoryList { get; set; }
    }
}

