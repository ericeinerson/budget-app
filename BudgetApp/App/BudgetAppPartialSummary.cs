using System;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public partial class BudgetApp
	{
        private void ProcessBudgetSummaryMenuOption()
        {
            switch (Validator.Convert<int>("a budget summary option"))
            {
                case (int)BudgetSummaryOption.ViewCurrentBalance:
                    ViewBalance();
                    break;
                case (int)BudgetSummaryOption.UpdateCurrentBalance:
                    UpdateBalance();
                    break;
                case (int)BudgetSummaryOption.Logout:
                    AppScreen.LogoutProgress();
                    Run();
                    break;
                case (int)BudgetSummaryOption.GoBack:
                    GoBackToAppScreen();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessWishlistOption();
                    break;
            }
        }

        public void ViewBalance()
        {
            Console.WriteLine(selectedAccount.Balance);
            Utilities.PressEnterToContinue();
        }

        public void UpdateBalance()
        {
            var _balance = Validator.Convert<decimal>("updated balance");
            selectedAccount.Balance = _balance;
            Console.WriteLine(_balance);
            //_currentBalance = Validator.Convert<decimal>("current balance");
            //Console.WriteLine($"\nYour current balance is {Utilities.FormatAmount(_currentBalance)}");
        }
    }
}

