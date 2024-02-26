using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetApp.App
{
	public partial class BudgetApp
    {
        public void VerifyTransactionStatus()
        {
            var transactionsFlagged = selectedAccount.TransactionList.Where(t => (t.Status == Status.Pending || t.Status == Status.Scheduled) && t.ScheduledDate <= DateTime.Now.AddMonths(1)).ToList();
            AppScreen.DisplayPostingOptions();
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    PostAllFlaggedTransactions(transactionsFlagged);
                    break;
                case 2:
                    IndividuallySelectTransactionStatus(transactionsFlagged);
                    break;
                case 3:
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessWishlistOption();
                    break;
            }

            Utilities.PressEnterToContinue();
        }

        public void AddSingleTransaction(BudgetItem item, BudgetItemType type)
        {

            Transaction transaction = ConstructSingleTransaction(item, type);
            selectedAccount.TransactionList.Add(transaction);
            Utilities.PrintMessage($"Successfully created transaction with " +
                $"name: {transaction.Name}, " +
                $"amount: {transaction.Amount} " +
                $"budget item id: {transaction.BudgetItemId}," +
            $"id: {transaction.Id}!", true, false);
        }

        public void CreateMultipleTransactions(BudgetItem item, BudgetItemType type)
        {
            var curDate = item.StartDate;
            var daysBetweenTransactions = 0;
            var postedDateString = string.Empty;

            switch (item.Rate)
            {
                case Rate.Weekly:
                    daysBetweenTransactions = 7;
                    break;
                case Rate.Biweekly:
                    daysBetweenTransactions = 14;
                    break;
                case Rate.Monthly:
                    break;
                case Rate.Yearly:
                    daysBetweenTransactions = 365;
                    break;
                case Rate.NoRate:
                case Rate.Other:
                    Console.WriteLine($"Cannot predict rate for other or no rate");
                    break;
                default:
                    throw new Exception();
            }

            while (curDate < item.EndDate)
            {
                var transaction = new Transaction();
                transaction.Id = AssignTransactionId();
                transaction.Name = item.Name;
                transaction.CategoryId = item.CategoryId;
                transaction.Amount = item.Amount;
                transaction.BudgetItemId = item.Id;
                transaction.BudgetItemType = type;
                transaction.CreatedDate = DateTime.Now;
                transaction.ScheduledDate = curDate;

                if (item.AmountVariable)
                {
                    Console.WriteLine($"Name: {item.Name}, Date: {curDate}, BudgetItemId: {item.Id}, Current Amount: {item.Amount}, Budget Item Type: {type}");
                    string isDifferentPrompt = Utilities.PromptYesOrNo($"Is this transaction different than {item.Amount}?");
                    if(isDifferentPrompt == "y")
                    {
                        transaction.Amount = Validator.Convert<decimal>("amount for this transaction");
                    }
                    Console.WriteLine();
                }
                if (item.Rate == Rate.Monthly)
                {
                    daysBetweenTransactions = DateTime.DaysInMonth(curDate.Year, curDate.Month);
                }
                if (curDate <= DateTime.Now)
                {
                    transaction.PostedDate = DateTime.Now;
                    transaction.Status = Status.Posted;
                    postedDateString = transaction.PostedDate.ToString();
                }
                else
                {
                    transaction.PostedDate = null;
                    transaction.Status = Status.Scheduled;
                    postedDateString = "Not Posted Yet";
                }
                selectedAccount.TransactionList.Add(transaction);      

                curDate = curDate.AddDays(daysBetweenTransactions);

                Utilities.PrintMessage($"Successfully created transaction! Id: " +
                    $"{transaction.Id}, " +
                    $"Name: {transaction.Name}, " +
                    $"Category Id: {transaction.CategoryId}, " +
                    $"Amount: {transaction.Amount}, " +
                    $"Budget Item Id: {transaction.BudgetItemId}," +
                    $"Budget Item Type: {transaction.BudgetItemType}," +
                    $"Created Date: {transaction.CreatedDate}," +
                    $"Name: {transaction.ScheduledDate}," +
                    $"Name: {postedDateString}," +
                    $"Status: {transaction.Status}", true, true);
                Console.WriteLine();
            }
        }

        private int AssignTransactionId()
        {
            selectedAccount.TransactionIdCounter++;
            return selectedAccount.TransactionIdCounter;
        }

        private void IndividuallySelectTransactionStatus(List<Transaction> transactionsFlagged)
        {
            foreach (var t in transactionsFlagged)
            {
                if ((t.Status == Status.Pending || t.Status == Status.Scheduled) && t.ScheduledDate <= DateTime.Now)
                {
                    string promptToPost = Utilities.PromptYesOrNo($"Post this transaction? Name: {t.Name} Amount: {t.Amount} Id: {t.Id}");
                    if (promptToPost == "y")
                    {
                        t.Status = Status.Posted;
                    }
                    else
                    {
                        t.Status = Status.Cancelled;
                        Utilities.PrintMessage("Transaction cancelled", false, true);
                    }
                }

                if (t.Status == Status.Scheduled && t.ScheduledDate <= DateTime.Now.AddMonths(1) && t.ScheduledDate > DateTime.Now)
                {
                    string promptToPending = Utilities.PromptYesOrNo($"Post this transaction? Name: {t.Name} Amount: {t.Amount} Id: {t.Id}");
                    if (promptToPending == "y")
                    {
                        t.Status = Status.Pending;
                    }
                }
            }
        }

        private void PostAllFlaggedTransactions(List<Transaction> transactionsFlagged)
        {
            foreach(var transaction in transactionsFlagged)
            {
                transaction.Status = Status.Posted;
                Console.WriteLine($"transaction posted: {transaction.Name} {transaction.Amount} {transaction.Id}");
            }
        }

        private Transaction ConstructSingleTransaction(BudgetItem item, BudgetItemType type)
        {
            if(item.Rate == Rate.Other)
            {
                Utilities.PrintMessage("WARNING: This transaction is being associated with an irregular rate. Make sure this isn't duplicated or incorrect", false, true);
            }
            else if(item.Rate != Rate.NoRate)
            {
                throw new Exception("This method can only be used for budget items with no rate or an other rate");
            }
            var transaction = new Transaction();
            
            int id;
            string name = item.Name;
            decimal amount = item.Amount;
            int categoryId = item.CategoryId;
            int budgetItemId = item.Id;
            BudgetItemType budgetItemType = BudgetItemType.None;
            DateTime createdDate = DateTime.Now;
            Status status = Status.None;

            id = selectedAccount.TransactionIdCounter;
            selectedAccount.TransactionIdCounter++;

            string postedToday = Utilities.PromptYesOrNo("Is this transaction posted today?");

            if(postedToday == "y")
            {
                transaction.ScheduledDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                transaction.Status = Status.Posted;
            }
            else
            {
                var scheduledMonth = Validator.Convert<int>("month of transaction to be scheduled");
                var scheduledDay = Validator.Convert<int>("day of transaction to be scheduled");
                var scheduledYear = Validator.Convert<int>("year of transaction to be scheduled");
                transaction.ScheduledDate = new DateTime(scheduledYear, scheduledMonth, scheduledDay);

                if (transaction.ScheduledDate <= DateTime.Now.AddMonths(1))
                {
                    status = Status.Pending;
                }
                else
                {
                    status = Status.Scheduled;
                }
            }

            if (type == BudgetItemType.Income)
            {
                budgetItemType = BudgetItemType.Income;
            }
            else if(type == BudgetItemType.Expense)
            {
                budgetItemType = BudgetItemType.Expense;
            }

            transaction.Id = id;
            transaction.CategoryId = categoryId;
            transaction.BudgetItemId = budgetItemId;
            transaction.Name = name;
            transaction.Amount = amount;
            transaction.CreatedDate = createdDate;
            transaction.BudgetItemType = budgetItemType;
            transaction.Status = status;

            return transaction;
        }

        public void UpdateTransactionsForTimePeriod(int daysInPast = 183, int daysInFuture = 365)
        {
            var transactionsExpected = new List<Transaction>();
            var rate = new Rate();
            var transactionDay = new DateTime();
            var daysBetweenTransactions = 0;
            var transactionsTable = new ConsoleTable("Name", "Created Date", "Amount", "Category Id", "Budget Item Id");
            foreach(var expense in selectedAccount.ExpenseList)
            {
                transactionsExpected = new List<Transaction>();
                rate = expense.Rate;
                transactionDay = expense.StartDate;
                daysBetweenTransactions = 0;

                switch (rate)
                {
                    case Rate.Weekly:
                        daysBetweenTransactions = 7;
                        break;
                    case Rate.Biweekly:
                        daysBetweenTransactions = 14;
                        break;
                    case Rate.Monthly:
                        break;
                    case Rate.Yearly:
                        daysBetweenTransactions = 365;
                        break;
                    case Rate.NoRate:
                    case Rate.Other:
                        Console.WriteLine($"Cannot predict rate for {expense.Name} ");
                        continue;
                    default:
                        throw new Exception();
                }

                while (transactionDay < DateTime.Now.AddDays(-1 * daysInPast))
                {
                    if(rate == Rate.Weekly || rate == Rate.Biweekly)
                    {
                        transactionDay = transactionDay.AddDays(daysBetweenTransactions);
                    }
                    else if(rate == Rate.Yearly)
                    {
                        if (DateTime.IsLeapYear(transactionDay.Year))
                        {
                            daysBetweenTransactions = 366;
                        }
                        transactionDay = transactionDay.AddDays(daysBetweenTransactions);
                    }
                    else if(rate == Rate.Monthly)
                    {
                        transactionDay = transactionDay.AddDays(DateTime.DaysInMonth(transactionDay.Year, transactionDay.Month));
                    }
                    else if (rate == Rate.Other || rate == Rate.NoRate)
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                while(transactionDay < DateTime.Now.AddDays(daysInFuture))
                {
                    var transaction = new Transaction()
                    {
                        Name = expense.Name,
                        CreatedDate = transactionDay,
                        Amount = expense.Amount,
                        CategoryId = expense.CategoryId,
                        BudgetItemId = expense.Id,
                    };
                    transactionsExpected.Add(transaction);

                    transactionsTable.AddRow(transaction.Name, transaction.CreatedDate, Utilities.FormatAmount(transaction.Amount), transaction.CategoryId, transaction.BudgetItemId);

                    if (rate == Rate.Biweekly || rate == Rate.Weekly)
                    {
                        transactionDay = transactionDay.AddDays(daysBetweenTransactions);
                    }
                    else if (rate == Rate.Yearly)
                    {
                        if (DateTime.IsLeapYear(transactionDay.Year))
                        {
                            daysBetweenTransactions = 366;
                        }
                        transactionDay = transactionDay.AddDays(daysBetweenTransactions);
                    }
                    else if (rate == Rate.Monthly)
                    {
                        transactionDay = transactionDay.AddDays(DateTime.DaysInMonth(transactionDay.Year, transactionDay.Month));
                    }
                    else if(rate == Rate.Other || rate == Rate.NoRate)
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                transactionsTable.AddRow("break", "", "", "", "");
            }
            transactionsTable.Write();
            Console.WriteLine("'\n'\n'\n....\n'  '\n'  '\n'  '\n'  '");
        }

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

