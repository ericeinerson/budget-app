using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;

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
                case (int)BudgetSummaryOption.ViewMainSummary:
                    ViewMainBudgetSummary();
                    break;
                case (int)BudgetSummaryOption.Logout:
                    LogoutProgress();
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

        public void ViewMainBudgetSummary(int? year = null)
        {
            if(year == null)
            {
                year = DateTime.Now.Year;
            }

            var mainTable = new ConsoleTable($"Main Summaries for year of {year}");
            var expensesRow = CalculateTotalForRestOfYear(BudgetItemType.Expense);
            var expensesRowFormatted = Utilities.FormatAmount(expensesRow);
            var incomesRow = CalculateTotalForRestOfYear(BudgetItemType.Income);
            var incomesRowFormatted = Utilities.FormatAmount(incomesRow);
            var currentBalance = Utilities.FormatAmount(selectedAccount.Balance);
            var projectedBalance = Utilities.FormatAmount(incomesRow - expensesRow + selectedAccount.Balance);

            mainTable.AddRow("Projected Remaining Total Expenses");
            mainTable.AddRow(expensesRowFormatted);
            mainTable.AddRow("Projected Remaining Total Incomes");
            mainTable.AddRow(incomesRowFormatted);
            mainTable.AddRow($"Projected Balance By End Of {year}");
            mainTable.AddRow(projectedBalance);
            mainTable.AddRow("Current Balance");
            mainTable.AddRow(currentBalance);

            mainTable.Write();
            Utilities.PressEnterToContinue();
        }

        public void ViewBalance()
        {
            var balanceTable = new ConsoleTable("Current Balance");
            var formattedBalance = Utilities.FormatAmount(selectedAccount.Balance);
            balanceTable.AddRow(formattedBalance);

            balanceTable.Write();

            Utilities.PressEnterToContinue();
        }

        public void UpdateBalance()
        {
            var _balance = Validator.Convert<decimal>("updated balance");
            selectedAccount.Balance = _balance;
            Console.WriteLine(Utilities.FormatAmount(_balance));
            Utilities.PressEnterToContinue();
        }

        public decimal CalculateTotalForRestOfYear(BudgetItemType type)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var endDate = new DateTime(DateTime.Now.Year, 12, 31);
            var totalValue = 0.00M;

            var transactionsList = selectedAccount.TransactionList.Where(e => e.ScheduledDate >= startDate && e.ScheduledDate <= endDate && e.BudgetItemType == type);

            foreach (Transaction transaction in transactionsList)
            {
                totalValue += transaction.Amount;
            }

            return totalValue;
        }
    }
}

