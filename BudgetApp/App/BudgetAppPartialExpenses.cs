using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;
using System.Globalization;

namespace BudgetApp.App
{
    public partial class BudgetApp
    {
        private void ProcessExpenseMenuOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case (int)ExpenseOption.ViewExpenses:
                    ViewExpenses();
                    break;
                case (int)ExpenseOption.AddExpense:
                    AddExpense();
                    break;
                case (int)ExpenseOption.RemoveExpense:
                    RemoveExpense();
                    break;
                //case (int)ExpenseOption.UpdateExpense:
                //    // TO DO
                //    Console.WriteLine("Add method for updating expense details");
                //    break;
                case (int)ExpenseOption.Logout:
                    AppScreen.LogoutProgress();
                    Run();
                    break;
                case (int)ExpenseOption.GoBack:
                    GoBackToAppScreen();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessWishlistOption();
                    break;
            }
        }

        public void ViewExpenses()
        {
            ConsoleTable allExpensesTable = new ConsoleTable("Name", "Amount","Date", "Id", "Category");
            foreach (Expense expense in selectedAccount.ExpenseList)
            {
                TransactionCategory? category = selectedAccount.TransactionCategoryList.Find(t => t.Id == expense.CategoryId);
                var categoryName = category != null ? category.Name : string.Empty;
                allExpensesTable.AddRow(expense.ExpenseName, expense.AmountFormatted, expense.Date.ToString("MMMM dd, yyyy"), expense.Id, categoryName);
            }
            allExpensesTable.Write();
            Utilities.PressEnterToContinue();
        }

        public void AddExpense()
        {
            Expense expense = ConstructExpense();
            selectedAccount.ExpenseList.Add(expense);

            Utilities.PrintMessage($"You have succcessfully added {expense.ExpenseName} with a value of {expense.AmountFormatted}. This expense will be done on {expense.Date.ToString("MMMM dd, yyyy")}!", true, false);
        }

        public void RemoveExpense()
        {
            Expense expense = FindExpense();
            if (expense != null)
            {
                selectedAccount.ExpenseList.Remove(expense);
                Utilities.PrintMessage($"You have succcessfully removed {expense.ExpenseName} with a value of {expense.AmountFormatted}!", true, false);
            }
        }

        public Expense FindExpense()
        {
            Expense? expense = null;

            while(expense == null)
            {
                string expenseName = Utilities.GetUserInput("expense name. If not known, enter n to skip or a to exit to app menu").ToLower();
                if(expenseName == "a")
                {
                    break;
                }
                expense = selectedAccount.ExpenseList.FirstOrDefault(e => e.ExpenseName.ToLower() == expenseName.ToLower());
                if (expenseName == "n")
                {
                    decimal expenseAmount = Validator.Convert<decimal>("expense amount");
                    expense = selectedAccount.ExpenseList.FirstOrDefault(e => e.Amount == expenseAmount);
                }

                if(expense == null)
                {
                    Utilities.PrintMessage("Sorry, expense not found. Please try again", false, false);
                }
            }

            if(expense == null)
            {
                throw new NullReferenceException();
            }

            return expense;
        }

        public Expense ConstructExpense()
        {
            Expense expense = new Expense();
            expenseId++;
            int id = expenseId;
            string name = Utilities.GetUserInput("expense name");
            decimal amount = Validator.Convert<decimal>("expense amount");
            string formattedAmount = string.Format(new CultureInfo("en-US"), "{0:c}", amount);
            TransactionCategory category = AssignTransactionCategory();

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

            expense.Id = expenseId;
            expense.ExpenseName = name;
            expense.Amount = amount;
            expense.AmountFormatted = formattedAmount;
            expense.Rate = rate;
            expense.Date = date;
            expense.CategoryId = category == null ? 0 : category.Id;

            return expense;
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

        private void PayPartialExpense(string expenseName, decimal payment)
        {
            Expense? expense = selectedAccount.ExpenseList.Find(e => e.ExpenseName == expenseName);
            if(expense == null)
            {
                throw new NullReferenceException();
            }
            decimal newAmount = expense.Amount - payment;

            expense.Amount -= payment;
            Utilities.PrintMessage($"You have successfully paid {payment} towards {expenseName}. Your remaining expense amount is {Utilities.FormatAmount(newAmount)}", true);
        }

        private void UpdateMonthlyExpenseAmount(string expenseName)
        {
            Expense? expense = selectedAccount.ExpenseList.Find(e => e.ExpenseName == expenseName);

            if(expense == null)
            {
                throw new NullReferenceException();
            }

            decimal monthlyExpenseAmount = 0;

            monthlyExpenseAmount = Validator.Convert<decimal>("monthly expense amount");

            expense.Amount = monthlyExpenseAmount;

            Console.WriteLine($"The monthly amount for {expenseName} is {Utilities.FormatAmount(monthlyExpenseAmount)}");
        }

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
        void CalculateExpensesForEachRate()
        {
            decimal _weeklyExpenses = 0;
            decimal _biweeklyExpenses = 0;
            decimal _monthlyExpenses = 0;
            decimal _yearlyExpenses = 0;

            foreach (Expense expense in selectedAccount.ExpenseList)
            {
                switch (expense.Rate)
                {
                    case Rate.Weekly:
                        _weeklyExpenses += expense.Amount;
                        break;
                    case Rate.Biweekly:
                        _biweeklyExpenses += expense.Amount;
                        break;
                    case Rate.Monthly:
                        _monthlyExpenses += expense.Amount;
                        break;
                    case Rate.Yearly:
                        _yearlyExpenses += expense.Amount;
                        break;
                }
            }
        }
    }
}

