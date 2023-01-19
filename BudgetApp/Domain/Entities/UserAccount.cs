using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class UserAccount
	{
		public int Id { get; set; }
		public int Passcode { get; set; }
		public string? FullName { get; set; }
		public decimal Balance { get; set; }
		public int TotalLogin { get; set; }
		public bool IsLocked { get; set; }
		public decimal TotalExpenses { get; set; }
		public Dictionary<string, decimal>? ExpenseCategories { get; set; }
		public decimal TotalIncomes { get; set; }
		public Wishlist Wishlist {get;set;}
	}
}

