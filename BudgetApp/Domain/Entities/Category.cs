using System;
namespace BudgetApp.Domain.Entities
{
	public class Category
	{
		public Category()
		{
			Name = "test category";
			Id = 0;
		}

		public string Name { get; set; }
		public int Id { get; set; }
	}
}

