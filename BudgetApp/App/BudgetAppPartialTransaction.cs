using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public partial class BudgetApp
    {
        //public void VerifyTransactionStatus()
        //{
        //    var transactionsPending = new List<Transaction>();

        //    foreach (Transaction transaction in selectedAccount.ExpenseList)
        //    {
        //        if (transaction.Date <= DateTime.Now.AddDays(1) && transaction.Status == Status.Pending)
        //        {
        //            transactionsPending.Add(transaction);
        //        }
        //    }

        //    foreach (Transaction transaction in selectedAccount.IncomeList)
        //    {
        //        if (transaction.Date <= DateTime.Now.AddDays(1) && transaction.Status == Status.Pending)
        //        {
        //            transactionsPending.Add(transaction);
        //        }
        //    }
        //    if (!transactionsPending.Any())
        //    {
        //        return;
        //    }

        //    Console.WriteLine("Post these expenses?");
        //    foreach (Transaction transaction in transactionsPending)
        //    {
        //        Console.WriteLine($"{transaction.Name}, {transaction.Amount}, {transaction.Date}, {transaction.Id}");
        //    }
        //    Utilities.PressEnterToContinue();

        //    AppScreen.DisplayPostingOptions();
        //    switch (Validator.Convert<int>("an option"))
        //    {
        //        case 1:
        //            PostAllTransactions(transactionsPending);
        //            break;
        //        case 2:
        //            PostSomeTransactions(transactionsPending);
        //            break;
        //        case 3:
        //            break;
        //        default:
        //            Utilities.PrintMessage("Invalid Option. Try again", false);
        //            ProcessWishlistOption();
        //            break;
        //    }
        //}

        //public void PostAllTransactions(List<Transaction> transactionsPending)
        //{
        //    foreach(Transaction transaction in transactionsPending)
        //    {
        //        transaction.Status = Status.Posted;
        //    }
        //}

        //public void PostSomeTransactions(List<Transaction> transactionsPending)
        //{
        //    string goThroughEachTransaction = Utilities.PromptYesOrNo("Go through each transaction?");

        //    if (goThroughEachTransaction == "y")
        //    {
        //        foreach (Transaction transaction in transactionsPending)
        //        {
        //            Console.WriteLine($"{transaction.Name}, {transaction.Amount}, {transaction.Date}, {transaction.Id}");
        //            string prompt = Utilities.PromptYesOrNo("Post this transaction?");
        //            if (prompt == "y")
        //            {
        //                transaction.Status = Status.Posted;
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string selection = string.Empty;
        //        Console.WriteLine("Enter transaction id, name, amount, and/or date to post. Press d to exit");

        //        while (transactionsPending.Any() && selection.ToLower() != "d")
        //        {

        //            selection = Utilities.GetUserInput("a detail");

        //            var transactionFound = transactionsPending.Find(t => t.Name == selection);
        //            if(transactionFound == null)
        //            {
        //                transactionFound = transactionsPending.Find(t => t.Id.ToString() == selection);
        //            }
        //            if (transactionFound == null)
        //            {
        //                transactionFound = transactionsPending.Find(t => t.Amount.ToString() == selection);
        //            }
        //            if (transactionFound == null)
        //            {
        //                transactionFound = transactionsPending.Find(t => t.StartDate.ToString() == selection);
        //            }
        //            if (transactionFound != null)
        //            {
        //                Console.WriteLine($"{transactionFound.Name}, {transactionFound.Amount}, {transactionFound.Date}, {transactionFound.Id}");
        //                string postTransaction = Utilities.PromptYesOrNo("Post this transaction?");
        //                if(postTransaction == "y")
        //                {
        //                    transactionFound.Status = Status.Posted;
        //                    transactionsPending.Remove(transactionFound);
        //                    Console.WriteLine($"{transactionFound.Name} flipped from pending to posted!");
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"{transactionFound.Name} remains pending");
        //                }
        //            }
        //        }
        //    }
        //}

        //public void SetFutureTransactionsForNextYear(BudgetItem item)
        //{
        //    var transactionListExpenses = selectedAccount.ExpenseList.Select(t =>
        //    t.Date >= DateTime.Now.AddYears(-1) || t.Date <= DateTime.Now.AddYears(1));

        //    var transactionListIncomes = selectedAccount.IncomeList.Select(t =>
        //    t.Date >= DateTime.Now.AddYears(-1) || t.Date <= DateTime.Now.AddYears(1));


        //}

        //void CalculateExpensesForEachRate()
        //{
        //    decimal _weeklyExpenses = 0;
        //    decimal _biweeklyExpenses = 0;
        //    decimal _monthlyExpenses = 0;
        //    decimal _yearlyExpenses = 0;

        //    foreach (Expense expense in selectedAccount.ExpenseList)
        //    {
        //        switch (expense.Rate)
        //        {
        //            case Rate.Weekly:
        //                _weeklyExpenses += expense.Amount;
        //                break;
        //            case Rate.Biweekly:
        //                _biweeklyExpenses += expense.Amount;
        //                break;
        //            case Rate.Monthly:
        //                _monthlyExpenses += expense.Amount;
        //                break;
        //            case Rate.Yearly:
        //                _yearlyExpenses += expense.Amount;
        //                break;
        //        }
        //    }
        //}

        //decimal CalculateExpensesForTimePeriod(TimeRange timeRange, DateTime endTime)
        //{

        //    SumOfAllExpenses = 0;

        //    foreach (Expense expense in selectedAccount.ExpenseList)
        //    {
        //        SumOfAllExpenses += CalculateExpenseByRateAndTime(timeRange, expense, endTime);
        //    }

        //    CalculateExpensesForEachRate();

        //    return SumOfAllExpenses;
        //}

        //private void PayPartialExpense(string expenseName, decimal payment)
        //{
        //    Expense? expense = selectedAccount.ExpenseList.Find(e => e.ExpenseName == expenseName);
        //    if(expense == null)
        //    {
        //        throw new NullReferenceException();
        //    }
        //    decimal newAmount = expense.Amount - payment;

        //    expense.Amount -= payment;
        //    Utilities.PrintMessage($"You have successfully paid {payment} towards {expenseName}. Your remaining expense amount is {Utilities.FormatAmount(newAmount)}", true);
        //}

        public void GetDatesForTransactions(BudgetItem item)
        {
            var listOfDates = new List<DateTime>();

            var endDay = Validator.Convert<int>("end day");
            var endMonth = Validator.Convert<int>("end month");
            var endYear = Validator.Convert<int>("end year");

            var endDate = new DateTime(endYear, endMonth, endDay);

            switch (item.Rate)
            {
                case Rate.Weekly:
                    break;
                case Rate.Biweekly:
                    break;
                case Rate.Monthly:
                    break;
                case Rate.Yearly:
                    break;
            }

            foreach (DateTime date in listOfDates)
            {
                Console.WriteLine(date.ToString("MMMM dd, yyyy"));
            }
        }

        private void ProcessBudgetItem(decimal amount)
        {
            Console.WriteLine("\nProcessing expense");
            Utilities.PrintDotAnimation();
            Console.WriteLine("");

            if (amount <= 0)
            {
                Utilities.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }

            if (PreviewUpdate(amount) == false)
            {
                Utilities.PrintMessage("You have cancelled your action", false);
                return;
            }
        }
    }
}

