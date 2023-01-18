using System;
namespace BudgetApp.Domain.Entities.Interfaces
{
	public interface IUserAccountActions
	{
		void BudgetSummary();
		//void PreviousMonths();
		void Incomes();
		void CategorizedExpenses();
		//void Wishlist();
	}
}

