using System;
using System.Threading;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public class Entry
	{
        static void Main(string[] args)
        {
            BudgetApp budgetApp = new BudgetApp();
            budgetApp.InitializeData();
            budgetApp.Run();
        }
    }
}

