using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum TransactionOption
	{
        [Description("View Transactions")]
        ViewTransactions = 1,
        [Description("Add Transaction")]
        AddTransaction,
        [Description("Remove Transaction")]
        RemoveTransaction,
        [Description("Update A Transaction's Details")]
        UpdateTransactionDetails,
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
	}
}

