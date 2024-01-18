using System;
namespace BudgetApp.Domain.Entities
{
	public class TransactionCategory
	{
		public TransactionCategory()
		{
			Name = "test category";
		}

		public string Name { get; set; }
		public int Id { get; set; }
	}
}

