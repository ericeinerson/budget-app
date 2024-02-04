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
			IncomeIdCounter = 0;
			ExpenseIdCounter = 0;
			BudgetItemIdCounter = 0;
			TransactionIdCounter = 0;
			WishlistIdCounter = 0;
            TransactionIdCounter = 0;
            Wishlist = new Wishlist();
			IncomeList = new List<BudgetItem>();
			ExpenseList = new List<BudgetItem>();
			TransactionList = new List<Transaction>();
			CategoryList = new List<Category>();
		}
		public int Id { get; set; }
		public string Directory { get; set; }
		public string Passcode { get; set; }
		public string FullName { get; set; }
		public decimal Balance { get; set; }
		public int TotalLogin { get; set; }
		public bool IsLocked { get; set; }
		public int IncomeIdCounter { get; set; }
		public int ExpenseIdCounter { get; set; }
		public int BudgetItemIdCounter { get; set; }
		public int TransactionIdCounter { get; set; }
		public int WishlistIdCounter { get; set; }
		public Wishlist Wishlist {get;set;}
		public List<BudgetItem> IncomeList { get; set; }
        public List<BudgetItem> ExpenseList { get; set; }
		public List<Category> CategoryList { get; set; }
		public List<Transaction> TransactionList { get; set; }
		public DateTime LastLoginDate { get; set; }
    }
}

