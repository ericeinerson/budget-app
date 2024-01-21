using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum IncomeOption
	{
        [Description("View Incomes")]
        ViewIncomes = 1,
        [Description("Add Income")]
        AddIncome,
        [Description("Remove Income")]
        RemoveIncome,
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
	}
}

