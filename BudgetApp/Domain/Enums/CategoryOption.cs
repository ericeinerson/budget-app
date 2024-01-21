using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum CategoryOption
	{
        [Description("View Categories")]
        ViewCategories = 1,
        [Description("Add Category")]
        AddCategory,
        [Description("Remove Category")]
        RemoveCategory,
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
	}
}

