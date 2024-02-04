using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum BudgetItemOption
	{
        [Description("View Budget Items")]
        ViewBudgetItem = 1,
        [Description("Add A Budget Item")]
        AddBudgetItem,
        [Description("Remove A Budget Item")]
        RemoveBudgetItem,
        [Description("Update A Budget Item's Details")]
        UpdateBudgetItemDetails,
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
	}
}

