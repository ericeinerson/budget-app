using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum AppMenu
	{
		[Description("Budget Summary")]
		BudgetSummary = 1,
        [Description("Instructions")]
        Instructions,
        [Description("Incomes")]
        Incomes,
        [Description("Expenses")]
        Expenses,
        [Description("Categories")]
        Categories,
        [Description("Wishlist")]
        Wishlist,
        [Description("Logout")]
        Logout,
        [Description("Save Info")]
        SaveInfo,
        [Description("Load Info")]
        LoadInfo
    }
}

