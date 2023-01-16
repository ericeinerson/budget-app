using System;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities
{
	public class BudgetUpdate
	{
		public long UpdateId { get; set; }
		public string UpdateDate { get; set; }
		public UpdateType UpdateType { get; set; }
		public string Description { get; set; }
		public Decimal UpdateAmount { get; set; }
	}
}

