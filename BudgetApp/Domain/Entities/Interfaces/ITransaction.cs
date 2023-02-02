using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities.Interfaces
{
	public interface ITransaction
	{
		void InsertTransaction(long _userAccountId, TransactionType _updateType, decimal _updateAmount, string description);
		void ViewTransactions();
	}
}

