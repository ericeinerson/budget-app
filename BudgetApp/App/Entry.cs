using System;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public class Entry
	{
        static void Main(string[] args)
        {
            AppScreen.Welcome();
            BudgetApp budgetApp = new BudgetApp();
            budgetApp.InitializeData();
            budgetApp.CheckUserPasscode();
            budgetApp.Welcome();
            Utilities.PressEnterToContinue();
        }
    }
}

