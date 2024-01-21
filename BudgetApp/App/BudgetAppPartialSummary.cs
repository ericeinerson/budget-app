using System;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public partial class BudgetApp
	{
        public void ProcessBudgetSummaryOption()
        {
            AppScreen.DisplayBudgetSummaryOptions();
            ProcessBudgetSummaryMenuOption();
        }

        private void ProcessBudgetSummaryMenuOption()
        {
            switch (Validator.Convert<int>("a budget summary option"))
            {
                //case 1:
                //    ShowBudgetForCurrentMonthAndYear();
                //    break;
                //case 2:
                //    ShowBudgetForOtherTimeRange();
                //    break;
                //case 3:
                //    ViewTransactions();
                //    break;
                case 8:
                    AppScreen.LogoutProgress();
                    Run();
                    break;
                case 9:
                    GoBackToAppScreen();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessWishlistOption();
                    break;
            }
        }

        public void UpdateBalance()
        {
            Console.WriteLine("Please enter your balance");
            //_currentBalance = Validator.Convert<decimal>("current balance");
            //Console.WriteLine($"\nYour current balance is {Utilities.FormatAmount(_currentBalance)}");

        }
    }
}

