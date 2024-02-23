using System;
namespace BudgetApp.Domain.Entities
{
	public class Income : BudgetItem
	{
		public Income()
		{
			IncomeId = -1;
		}
		public int IncomeId { get; set; }
	}
}

