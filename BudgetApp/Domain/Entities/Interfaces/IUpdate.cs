using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities.Interfaces
{
	public interface IUpdate
	{
		void InsertUpdate(UpdateType _updateType, decimal _updateAmount, string description);
		void ViewUpdate();
	}
}

