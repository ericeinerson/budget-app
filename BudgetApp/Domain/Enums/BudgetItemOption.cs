using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	
    public enum BudgetItemOption
    {
        [Description("View Items")]
        ViewBudgetItem = 1,
        [Description("Add An Item")]
        AddBudgetItem,
        [Description("Remove An Item")]
        RemoveBudgetItem,
        [Description("Update An Item's Details")]
        UpdateBudgetItemDetails,
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
    }
}

