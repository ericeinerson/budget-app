using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;
using System.Globalization;
using BudgetApp.Extensions;

namespace BudgetApp.App
{
    public partial class BudgetApp
    {
        private void ProcessTransactionMenuOption(TransactionType transactionType)
        {
            switch (Validator.Convert<int>("an option"))
            {
                case (int)TransactionOption.ViewTransactions:
                    ViewTransactions(transactionType);
                    break;
                case (int)TransactionOption.AddTransaction:
                    AddTransaction(transactionType);
                    break;
                case (int)TransactionOption.RemoveTransaction:
                    RemoveTransaction(transactionType);
                    break;
                case (int)TransactionOption.UpdateTransactionDetails:
                    UpdateTransactionDetails(transactionType);
                    break;
                case (int)TransactionOption.Logout:
                    LogoutProgress();
                    break;
                case (int)TransactionOption.GoBack:
                    GoBackToAppScreen();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessWishlistOption();
                    break;
            }
        }

        public void ViewTransactions(TransactionType transactionType)
        {
            ConsoleTable allTransactionsTable = new("Name", "Amount","Date", "Id", "Category", "Rate", "Transaction Type");

            foreach (Transaction transaction in GetTransactionList(transactionType))
            {
                Category? category = selectedAccount.CategoryList.Find(t => t.Id == transaction.CategoryId);
                var categoryName = category != null ? category.Name : string.Empty;
                allTransactionsTable.AddRow(transaction.Name, transaction.AmountFormatted, transaction.Date.ToString("MMMM dd, yyyy"), transaction.Id, categoryName, transaction.Rate.GetDescription(), transaction.TransactionType);
            }
            allTransactionsTable.Write();
            Utilities.PressEnterToContinue();
        }

        public List<Transaction> GetTransactionList(TransactionType transactionType)
        {
            var transactionList = new List<Transaction>();

            if (transactionType == TransactionType.Expense)
            {
                transactionList = selectedAccount.ExpenseList;
            }
            else if (transactionType == TransactionType.Income)
            {
                transactionList = selectedAccount.IncomeList;
            }
            else
            {
                throw new Exception();
            }

            return transactionList;
        }

        public void AddTransaction(TransactionType transactionType)
        {
            Transaction transaction = ConstructTransaction(transactionType);

            var transactionList = GetTransactionList(transactionType);

            transactionList.Add(transaction);

            Utilities.PrintMessage($"You have succcessfully added {transaction.Name} with a value of {transaction.AmountFormatted}. This transaction will be done on {transaction.Date.ToString("MMMM dd, yyyy")}!", true, false);
        }

        public void RemoveTransaction(TransactionType transactionType)
        {
            Transaction transaction = FindTransaction(transactionType);

            var transactionList = GetTransactionList(transactionType);

            if (transaction != null)
            {
                transactionList.Remove(transaction);
                Utilities.PrintMessage($"You have succcessfully removed {transaction.Name} with a value of {transaction.AmountFormatted}!", true, false);
            }
        }

        public Transaction FindTransaction(TransactionType transactionType)
        {
            Transaction? transaction = null;
            var transactionList = GetTransactionList(transactionType);
            while(transaction == null)
            {
                string transactionName = Utilities.GetUserInput("name. If not known, enter n to skip or a to exit to app menu").ToLower();
                if(transactionName == "a")
                {
                    break;
                }
                transaction = transactionList.FirstOrDefault(t => t.Name.ToLower() == transactionName.ToLower());
                if (transactionName == "n")
                {
                    decimal amount = Validator.Convert<decimal>("amount");
                    transaction = transactionList.FirstOrDefault(t => t.Amount == amount);
                }

                if(transaction == null)
                {
                    Utilities.PrintMessage("Sorry, transaction not found. Please try again", false, false);
                }
            }

            if(transaction == null)
            {
                throw new NullReferenceException();
            }

            return transaction;
        }

        public Transaction ConstructTransaction(TransactionType transactionType)
        {
            Transaction transaction = new Transaction();
            var transactionList = GetTransactionList(transactionType);
            int id;

            if (transactionType == TransactionType.Expense)
            {
                selectedAccount.ExpenseId++;
                id = selectedAccount.ExpenseId;
            }
            else if (transactionType == TransactionType.Income)
            {
                selectedAccount.IncomeId++;
                id = selectedAccount.ExpenseId;
            }
            else
            {
                throw new Exception();
            }
            string name = Utilities.GetUserInput("name");
            decimal amount = Validator.Convert<decimal>("amount");
            string formattedAmount = string.Format(new CultureInfo("en-US"), "{0:c}", amount);
            Category category = AssignTransactionCategory();

            //TEST CODE REMOVE LATER START
            if (category != null)
            {
                var categoryId = category.Id;
                var categoryName = category.Name;

                Console.WriteLine(categoryId);
                Console.WriteLine(categoryName);
            }
            else
            {
                Console.WriteLine("Successfully exited the while loop with q");
            }
            Utilities.PressEnterToContinue();
            //TEST CODE REMOVE LATER END

            Rate rate = ProcessRateOption();
            DateTime date = DateTime.MinValue;
            string todayOrFuture = Utilities.GetUserInput("whether t for today or f for future").ToLower();

            if (todayOrFuture == "t")
            {
                date = DateTime.Now;
            }
            else if (todayOrFuture == "f")
            {
                date = Utilities.ConstructDate();
            }

            transaction.Id = id;
            transaction.Name = name;
            transaction.Amount = amount;
            transaction.AmountFormatted = formattedAmount;
            transaction.Rate = rate;
            transaction.Date = date;
            transaction.CategoryId = category == null ? 0 : category.Id;

            return transaction;
        }

        private void UpdateTransactionDetails(TransactionType transactionType)
        {
            var transaction = FindTransaction(transactionType);

            AppScreen.DisplayTransactionUpdateDetails();

            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    UpdateTransactionAmount(transaction);
                    break;
                case 2:
                    UpdateTransactionName(transaction);
                    break;
                case 3:
                    UpdateTransactionRate(transaction);
                    break;
                case 4:
                    UpdateTransactionDate(transaction);
                    break;
                case 5:
                    UpdateTransactionCategory(transaction);
                    break;
                case 6:
                    UpdateAllTransactionDetails(transaction);
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    UpdateTransactionDetails(transactionType);
                    break;
            }
        }

        private void UpdateTransactionAmount(Transaction transaction)
        {
            var amount = Validator.Convert<decimal>("new amount");
            transaction.Amount = amount;
            transaction.AmountFormatted = string.Format(new CultureInfo("en-US"), "{0:c}", amount);
        }

        private void UpdateTransactionName(Transaction transaction)
        {
            var name = Utilities.GetUserInput("new name");
            transaction.Name = name;
        }

        private void UpdateTransactionRate(Transaction transaction)
        {
            var rate = ProcessRateOption();
            transaction.Rate = rate;
        }

        private void UpdateTransactionDate(Transaction transaction)
        {
            var date = Utilities.ConstructDate();
            transaction.Date = date;
        }

        private void UpdateTransactionCategory(Transaction transaction)
        {
            var categoryId = AssignTransactionCategory().Id;
            transaction.CategoryId = categoryId;
        }

        private void UpdateAllTransactionDetails(Transaction transaction)
        {
            UpdateTransactionAmount(transaction);
            UpdateTransactionName(transaction);
            UpdateTransactionRate(transaction);
            UpdateTransactionDate(transaction);
            UpdateTransactionCategory(transaction);
        }
        //public string ProcessOtherExpense()
        //{
        //    Console.WriteLine("Would you like to add a new category (Y/N)?\n");
        //    string otherCategory = string.Empty;
        //    string newCategory = string.Empty;

        //    while (true)
        //    {
        //        otherCategory = Console.ReadLine();
        //        if (otherCategory == "Y")
        //        {
        //            newCategory = Utilities.GetUserInput("new category");

        //            if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == newCategory))
        //            {
        //                Utilities.PrintMessage("Enter the following information to add to list:", true);

        //                selectedAccount.ExpenseList.Add(ConstructExpense(newCategory));
        //            }

        //            Utilities.PrintMessage($"You have added {newCategory} to your list of expenses", true);

        //            Console.WriteLine("Expense categories:\n");

        //            foreach (Expense expense in selectedAccount.ExpenseList)
        //            {
        //                Console.WriteLine(expense.ExpenseName);
        //            }
        //            return newCategory;
        //        }
        //        else if (otherCategory == "N")
        //        {
        //            Console.WriteLine("Would you like to use an existing other category (Y/N)?\n");

        //            while (true)
        //            {
        //                otherCategory = Console.ReadLine();

        //                if (otherCategory == "Y")
        //                {
        //                    while (true)
        //                    {
        //                        Console.WriteLine("Choose from the categories below:\n");
        //                        foreach (Expense category in selectedAccount.ExpenseList)
        //                        {
        //                            Console.WriteLine(category.ExpenseName);
        //                        }

        //                        newCategory = Utilities.GetUserInput("other category name to be used");

        //                        if (selectedAccount.ExpenseList.Any(e => e.ExpenseName == newCategory))
        //                        {
        //                            return newCategory;
        //                        }
        //                        else if (newCategory == "q")
        //                        {
        //                            AppScreen.LogoutProgress();
        //                            Utilities.PrintMessage("You have successfully logged out.", true);
        //                            Run();
        //                        }
        //                        else
        //                        {
        //                            Utilities.PrintMessage("Invalid input. Try again or press q to logout", false);
        //                            continue;
        //                        }
        //                    }

        //                }
        //                else if (otherCategory == "N")
        //                {
        //                    break;
        //                }
        //                else
        //                {
        //                    Utilities.PrintMessage("Invalid input. Try again", false, true);
        //                    Console.WriteLine("Enter Y/N");

        //                    continue;
        //                }
        //            }
        //            break;
        //        }
        //        else
        //        {
        //            Utilities.PrintMessage("Invalid input. Please try again", false, true);
        //            Console.WriteLine("Enter Y/N");
        //            continue;
        //        }
        //    }

        //    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "other"))
        //    {
        //        selectedAccount.ExpenseList.Add(ConstructExpense("other"));
        //    }
        //    Console.WriteLine("You have chosen other");
        //    return "other";
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

        //private void UpdateMonthlyExpenseAmount(string expenseName)
        //{
        //    Expense? expense = selectedAccount.ExpenseList.Find(e => e.ExpenseName == expenseName);

        //    if(expense == null)
        //    {
        //        throw new NullReferenceException();
        //    }

        //    decimal monthlyExpenseAmount = 0;

        //    monthlyExpenseAmount = Validator.Convert<decimal>("monthly expense amount");

        //    expense.Amount = monthlyExpenseAmount;

        //    Console.WriteLine($"The monthly amount for {expenseName} is {Utilities.FormatAmount(monthlyExpenseAmount)}");
        //}

        private void ProcessExpense(decimal expense_amt)
        {
            Console.WriteLine("\nProcessing expense");
            Utilities.PrintDotAnimation();
            Console.WriteLine("");

            if (expense_amt <= 0)
            {
                Utilities.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }

            if (PreviewUpdate(expense_amt) == false)
            {
                Utilities.PrintMessage("You have cancelled your action", false);
                return;
            }
        }
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

    }
}

