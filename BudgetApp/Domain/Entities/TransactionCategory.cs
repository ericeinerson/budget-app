using System;
namespace BudgetApp.Domain.Entities
{
	public class Category
	{
		public Category()
		{
			Name = "test category";
		}

		public string Name { get; set; }
		public int Id { get; set; }
	}
}

