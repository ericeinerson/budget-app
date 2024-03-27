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
            Console.WriteLine("Would you like to use app test data or load data from a directory?");
            var dataLoadMethod = Utilities.GetUserInput("t for test data or d for directory");
            while(dataLoadMethod != "t" && dataLoadMethod != "d")
            {
                Utilities.PrintMessage("Invalid Input. Please try again", false, false);
                dataLoadMethod = Utilities.GetUserInput("t for test data or d for directory");
            }
            if (dataLoadMethod == "d")
            {
                budgetApp.LoadDataFromDirectory();
            }
            else if (dataLoadMethod == "t")
            {
                budgetApp.LoadDataFromDirectory();
            }
            budgetApp.Run();
        }
    }
}

