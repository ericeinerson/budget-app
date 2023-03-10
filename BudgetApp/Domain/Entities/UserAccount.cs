using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class UserAccount
	{
		public UserAccount()
		{
			Id = 1;
			Passcode = 0000;
			FullName = "test name";
			Balance = 0;
			TotalLogin = 0;
			IsLocked = true;
			TotalExpenses = 0;
			TotalIncomes = 0;
			Wishlist = new Wishlist();
			IncomeList = new List<Income>();
			ExpenseList = new List<Expense>();
		}
		public int Id { get; set; }
		public int Passcode { get; set; }
		public string? FullName { get; set; }
		public decimal Balance { get; set; }
		public int TotalLogin { get; set; }
		public bool IsLocked { get; set; }
		public decimal TotalExpenses { get; set; }
		public decimal TotalIncomes { get; set; }
		public Wishlist Wishlist {get;set;}
		public List<Income> IncomeList { get; set; }
        public List<Expense> ExpenseList { get; set; }
    }
}

