using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities.Interfaces
{
	public interface ITransaction
	{
		void InsertTransaction(TransactionType _updateType, decimal _updateAmount, string description);
		void ViewTransaction();
	}
}

