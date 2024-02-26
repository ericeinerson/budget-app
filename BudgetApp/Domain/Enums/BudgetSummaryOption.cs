using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum BudgetSummaryOption
	{
		[Description("View Current Balance")]
		ViewCurrentBalance = 1,
        [Description("Update Current Balance")]
        UpdateCurrentBalance,
        [Description("View Summary of Expenses, Incomes, and Balance")]
        ViewMainSummary,
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
    }
}

