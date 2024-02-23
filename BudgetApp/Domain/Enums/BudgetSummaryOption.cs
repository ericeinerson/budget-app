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
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
    }
}

