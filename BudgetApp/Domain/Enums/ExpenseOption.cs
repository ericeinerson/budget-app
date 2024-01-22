using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum ExpenseOption
	{
        [Description("View Expenses")]
        ViewExpenses = 1,
        [Description("Add Expense")]
        AddExpense,
        [Description("Remove Expense")]
        RemoveExpense,
        [Description("Update An Expense's Details")]
        UpdateExpenseDetails,
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
	}
}

