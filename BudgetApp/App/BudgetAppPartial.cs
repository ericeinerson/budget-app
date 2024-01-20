using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Entities.Interfaces;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using BudgetApp.Domain;
using ConsoleTables;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetApp.App
{
    public partial class BudgetApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount>? userAccountList;
        protected UserAccount selectedAccount = new UserAccount();
        private int incomeId = 0;
        private int expenseId = 0;

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserPasscode();
            AppScreen.WelcomeCustomer(selectedAccount.FullName!);
            AppScreen.DisplayAppMenu();
            //Utilities.CheckForExistingUserFile(selectedAccount);
            ProcessAppMenuOption();
        }

        #region Initialize Data
        public void InitializeData()
        {
            UserAccount self = new UserAccount()
            {
                Id = 1,
                Directory = @"/Users/ericeinerson/Projects/BudgetApp/UserInfo/",
                FullName = "Eric Einerson",
                Passcode = "0991",
                Balance = 0,
                IsLocked = false,
                TotalLogin = 0,
                TotalIncomes = 0,
                ExpenseList = new List<Expense>(),
                Wishlist = new Wishlist(),
                IncomeList = new List<Income>(),
                TransactionCategoryList = new List<TransactionCategory>() { new TransactionCategory() { Name = "No Category", Id = 0 } }
            };

            userAccountList = new List<UserAccount>
            {
                self,
                new UserAccount
                {
                    Id = 2,
                    FullName = "Pickle Rick",
                    Passcode = "8374",
                    Balance = 0,
                    IsLocked = false,
                    TotalLogin = 0,
                    TotalExpenses = 0,
                    TotalIncomes = 0
                },
                new UserAccount
                {
                    Id = 3,
                    FullName = "Random User",
                    Passcode = "1111",
                    Balance = 0,
                    IsLocked = false,
                    TotalLogin = 0,
                    TotalExpenses = 0,
                    TotalIncomes = 0
                }
            };
            #endregion

            //_listOfTransactions = new List<Transaction>();
            //_incomeIdCounter = selectedAccount.IncomeList.Count;
        }

        #region Check Passcode
        public void CheckUserPasscode()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach (UserAccount account in userAccountList!)
                {
                    selectedAccount = account;
                    if (inputAccount.FullName.ToLower().Equals(selectedAccount.FullName.ToLower()))
                    {
                        selectedAccount.TotalLogin++;

                        if (inputAccount.Passcode.Equals(selectedAccount.Passcode))
                        {
                            selectedAccount = account;

                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }

                }
                if (isCorrectLogin == false)
                {
                    Utilities.PrintMessage("\nInvalid name or passcode", false);
                    selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                    if (selectedAccount.IsLocked)
                    {
                        AppScreen.PrintLockScreen();
                    }
                }
                Console.Clear();
            }
        }
        #endregion

        #region Process Menu Option
        protected void ProcessAppMenuOption()
        {
            switch (Validator.Convert<int>("an option."))
            {
                case (int)AppMenu.BudgetSummary:
                    BudgetSummary();
                    break;
                case (int)AppMenu.Instructions:
                    DisplayInstructions();
                    Utilities.PressEnterToContinue();
                    break;
                case (int)AppMenu.Incomes:
                    ProcessIncomeOption();
                    break;
                case (int)AppMenu.Expenses:
                    ProcessExpenseOption();
                    break;
                case (int)AppMenu.Categories:
                    ProcessCategoryOption();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    break;
                case (int)AppMenu.UpdateBalance:
                    UpdateBalance();
                    break;
                case (int)AppMenu.SaveInfo:
                    Utilities.SaveUserInformation(selectedAccount);
                    Utilities.PressEnterToContinue();
                    break;
                case (int)AppMenu.LoadInfo:
                    selectedAccount = Utilities.LoadUserInformation(selectedAccount);
                    Utilities.PressEnterToContinue();
                    break;
                default:
                    Utilities.PrintMessage("Invalid option.", false);
                    break;
            }
            AppScreen.DisplayAppMenu();
            ProcessAppMenuOption();
        }
        #endregion

        #region Budget Summary
        public void BudgetSummary()
        {
            AppScreen.DisplayBudgetSummaryOptions();
            ProcessBudgetSummaryMenu();
        }

        public void ProcessCategoryMenuOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    ViewTransactionCategories();
                    break;
                case 2:
                    AddTransactionCategory();
                    break;
                case 3:
                    RemoveTransactionCategory();
                    break;
            }
        }

        public void AddTransactionCategory()
        {
            TransactionCategory category = ConstructTransactionCategory();
            selectedAccount.TransactionCategoryList.Add(category);

            Utilities.PrintMessage($"You have succcessfully added {category.Name} with an id of {category.Id}!", true, false);
        }

        public void RemoveTransactionCategory()
        {
            TransactionCategory category = FindTransactionCategory();
            if (category != null && category.Id != 0)
            {
                selectedAccount.TransactionCategoryList.Remove(category);
                foreach(Expense expense in selectedAccount.ExpenseList)
                {
                    if(expense.CategoryId == category.Id)
                    {
                        expense.CategoryId = 0;
                    }
                }

                foreach (Income income in selectedAccount.IncomeList)
                {
                    if (income.CategoryId == category.Id)
                    {
                        income.CategoryId = 0;
                    }
                }
                Utilities.PrintMessage($"You have succcessfully removed {category.Name} with an id of {category.Id}!", true, false);
            }
            else if(category != null && category.Id == 0)
            {
                Utilities.PrintMessage("You cannot remove this category. It serves as a replacement for holding no categories", false, false);
            }
        }

        public TransactionCategory FindTransactionCategory()
        {
            TransactionCategory? category = null;

            while (category == null)
            {
                string categoryName = Utilities.GetUserInput("category name. If not known, enter n to skip or a to exit to app menu").ToLower();
                if (categoryName == "a")
                {
                    break;
                }
                category = selectedAccount.TransactionCategoryList.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
                if (categoryName == "n")
                {
                    int categoryId = Validator.Convert<int>("category id");
                    category = selectedAccount.TransactionCategoryList.FirstOrDefault(c => c.Id == categoryId);
                }

                if (category == null)
                {
                    Utilities.PrintMessage("Sorry, category not found. Please try again", false, false);
                }
            }

            return category;
        }

        public TransactionCategory ConstructTransactionCategory()
        {
            TransactionCategory category = new TransactionCategory();

            string categoryName = Utilities.GetUserInput("category name");
            int categoryId = 0;

            foreach(TransactionCategory tc in selectedAccount.TransactionCategoryList)
            {
                categoryId = Math.Max(categoryId, tc.Id);
            }
            categoryId++;

            category.Name = categoryName;
            category.Id = categoryId;

            return category;
        }

        public void ViewTransactionCategories()
        {
            ConsoleTable allCategoriesTable = new ConsoleTable("Name", "Id");
            foreach (TransactionCategory category in selectedAccount.TransactionCategoryList)
            {
                allCategoriesTable.AddRow(category.Name, category.Id);
            }
            allCategoriesTable.Write();
            Utilities.PressEnterToContinue();
        }

        public TransactionCategory AssignTransactionCategory()
        {
            TransactionCategory? category = null;
            
            var categoryList = selectedAccount.TransactionCategoryList;
            string? categoryProperty = Utilities.GetUserInput("category name or id. Press q to quit");

            while (category == null)
            {
                category = categoryList.FirstOrDefault(c => c.Id.ToString() == categoryProperty);

                if(category == null)
                {
                    category = categoryList.FirstOrDefault(c => c.Name == categoryProperty);
                }

                if (categoryProperty != null && categoryProperty.ToLower() == "q")
                {
                    break;
                }

                if (category == null)
                {
                    Utilities.PrintMessage("Category not found. Please try again", false, true);
                    categoryProperty = Console.ReadLine();
                }
            }

            return category;
        }

        private void ProcessExpenseOption()
        {
            AppScreen.DisplayExpenseOptions();
            ProcessExpenseMenuOption();
        }

        private void ProcessIncomeOption()
        {
            AppScreen.DisplayIncomeOptions();
            ProcessIncomeMenuOption();
        }

        private void ProcessCategoryOption()
        {
            AppScreen.DisplayCategoryOptions();
            ProcessCategoryMenuOption();
        }

        private void ProcessExpenseMenuOption()
        {
            switch(Validator.Convert<int>("an option"))
            {
                case 1:
                    ViewExpenses();
                    break;
                case 2:
                    AddExpense();
                    break;
                case 3:
                    RemoveExpense();
                    break;
                case 4:
                    // TO DO
                    Console.WriteLine("Add method for updating expense details");
                    break;
            }
        }

        private void ProcessIncomeMenuOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    ViewIncomes();
                    break;
                case 2:
                    AddIncome();
                    break;
                case 3:
                    RemoveIncome();
                    break;
                case 4:
                    // TO DO
                    Console.WriteLine("Add method for updating income details");
                    break;
                //case 5:
                //    GoToAppMenu();
                //    break;
            }
        }

        private void ProcessBudgetSummaryMenu()
        {
            switch(Validator.Convert<int>("a budget summary option"))
            {
                //case 1:
                //    ShowBudgetForCurrentMonthAndYear();
                //    break;
                //case 2:
                //    ShowBudgetForOtherTimeRange();
                //    break;
                case 3:
                    ViewTransactions();
                    break;
                case 4:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    break;
                case 5:
                    AppScreen.DisplayAppMenu();
                    ProcessAppMenuOption();
                    break;
            }
        }

        //public decimal CalculateIncomeByRateAndTime(TimeRange timeRange,Income income)
        //{
        //    DateTime currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
        //    DateTime currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
        //    DateTime startTimeSpan;
        //    DateTime endTimeSpan;

        //    DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, income.Month, income.Day);
        //    DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddHours(23);

        //    DateTime currentPayPeriod = firstPayPeriod;
        //    int payPeriodCounter = 0;
        //    int daysBetweenIntervals = 0;
        //    decimal pay = income.Amount;

        //    switch (income.Rate)
        //    {
        //        case Rate.Weekly:
        //            daysBetweenIntervals = 7;
        //            break;
        //        case Rate.Biweekly:
        //            daysBetweenIntervals = 14;
        //            break;
        //        case Rate.Monthly:
        //            daysBetweenIntervals = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        //            break;
        //        case Rate.Yearly:
        //            daysBetweenIntervals = DateTime.IsLeapYear(DateTime.Now.Year) ? 366: 365;
        //            break;
        //    }
        //    switch (timeRange)
        //    {
        //        case TimeRange.Month:
        //            startTimeSpan = currentMonthStart.AddMilliseconds(-1);
        //            endTimeSpan = currentMonthEnd;
        //            break;
        //        case TimeRange.Year:
        //            startTimeSpan = currentYearStart.AddMilliseconds(-1);
        //            endTimeSpan = currentYearEnd;
        //            break;
        //        case TimeRange.Other:
        //            startTimeSpan = DateTime.Now;
        //            int endTimeSpanDay = Validator.Convert<int>("end day");
        //            int endTimeSpanMonth = Validator.Convert<int>("end month");
        //            int endTimeSpanYear = Validator.Convert<int>("end year");
        //            endTimeSpan = new DateTime(endTimeSpanYear, endTimeSpanMonth, endTimeSpanDay);
        //            break;
        //        default:
        //            startTimeSpan = DateTime.MaxValue;
        //            endTimeSpan = DateTime.MinValue;
        //            break;
        //    }

        //    while(currentPayPeriod < startTimeSpan)
        //    {
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    while (currentPayPeriod < endTimeSpan)
        //    {
        //        payPeriodCounter++;
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    pay *= payPeriodCounter;
        //    return pay;
        //}

        //public decimal CalculateIncomeByRateAndTime(TimeRange timeRange, Income income, DateTime endTime)
        //{
        //    DateTime currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
        //    DateTime currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
        //    DateTime startTimeSpan;
        //    DateTime endTimeSpan;

        //    DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, income.Month, income.Day);
        //    DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddHours(23);

        //    DateTime currentPayPeriod = firstPayPeriod;
        //    int payPeriodCounter = 0;
        //    int daysBetweenIntervals = 0;
        //    decimal pay = income.Amount;

        //    switch (income.Rate)
        //    {
        //        case Rate.Weekly:
        //            daysBetweenIntervals = 7;
        //            break;
        //        case Rate.Biweekly:
        //            daysBetweenIntervals = 14;
        //            break;
        //        case Rate.Monthly:
        //            daysBetweenIntervals = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        //            break;
        //        case Rate.Yearly:
        //            daysBetweenIntervals = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
        //            break;
        //    }
            
        //    startTimeSpan = DateTime.Now;
        //    endTimeSpan = endTime;

        //    while (currentPayPeriod < startTimeSpan)
        //    {
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    while (currentPayPeriod < endTimeSpan)
        //    {
        //        payPeriodCounter++;
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    pay *= payPeriodCounter;
        //    return pay;
        //}

        //public decimal CalculateExpenseByRateAndTime(TimeRange timeRange, Expense expense, DateTime endTime)
        //{
        //    DateTime currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
        //    DateTime currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
        //    DateTime startTimeSpan;
        //    DateTime endTimeSpan;

        //    DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, expense.Month, expense.Day);
        //    DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddHours(23);

        //    DateTime currentPayPeriod = firstPayPeriod;
        //    int payPeriodCounter = 0;
        //    int daysBetweenIntervals = 0;
        //    decimal pay = expense.Amount;

        //    switch (expense.Rate)
        //    {
        //        case Rate.Weekly:
        //            daysBetweenIntervals = 7;
        //            break;
        //        case Rate.Biweekly:
        //            daysBetweenIntervals = 14;
        //            break;
        //        case Rate.Monthly:
        //            daysBetweenIntervals = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        //            break;
        //        case Rate.Yearly:
        //            daysBetweenIntervals = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
        //            break;
        //    }

        //    startTimeSpan = DateTime.Now;
        //    endTimeSpan = endTime;

        //    while (currentPayPeriod < startTimeSpan)
        //    {
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    while (currentPayPeriod < endTimeSpan)
        //    {
        //        payPeriodCounter++;
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    pay *= payPeriodCounter;
        //    return pay;
        //}

        //public decimal CalculateExpenseByRateAndTime(TimeRange timeRange, Expense expense)
        //{
        //    DateTime currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
        //    DateTime currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
        //    DateTime startTimeSpan;
        //    DateTime endTimeSpan;

        //    DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, expense.Month, expense.Day);
        //    DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddHours(23);

        //    DateTime currentPayPeriod = firstPayPeriod;
        //    int payPeriodCounter = 0;
        //    int daysBetweenIntervals = 0;
        //    decimal pay = expense.Amount;

        //    switch (expense.Rate)
        //    {
        //        case Rate.Weekly:
        //            daysBetweenIntervals = 7;
        //            break;
        //        case Rate.Biweekly:
        //            daysBetweenIntervals = 14;
        //            break;
        //        case Rate.Monthly:
        //            daysBetweenIntervals = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        //            break;
        //        case Rate.Yearly:
        //            daysBetweenIntervals = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
        //            break;
        //    }
        //    switch (timeRange)
        //    {
        //        case TimeRange.Month:
        //            startTimeSpan = currentMonthStart.AddMilliseconds(-1);
        //            endTimeSpan = currentMonthEnd;
        //            break;
        //        case TimeRange.Year:
        //            startTimeSpan = currentYearStart.AddMilliseconds(-1);
        //            endTimeSpan = currentYearEnd;
        //            break;
        //        case TimeRange.Other:
        //            startTimeSpan = DateTime.Now;
        //            int endTimeSpanDay = Validator.Convert<int>("end day");
        //            int endTimeSpanMonth = Validator.Convert<int>("end month");
        //            int endTimeSpanYear = Validator.Convert<int>("end year");
        //            endTimeSpan = new DateTime(endTimeSpanYear, endTimeSpanMonth, endTimeSpanDay);
        //            break;
        //        default:
        //            startTimeSpan = DateTime.MaxValue;
        //            endTimeSpan = DateTime.MinValue;
        //            break;
        //    }

        //    while (currentPayPeriod < startTimeSpan)
        //    {
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    while (currentPayPeriod < endTimeSpan)
        //    {
        //        payPeriodCounter++;
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    pay *= payPeriodCounter;
        //    return pay;
        //}

        public int CalculateTransactionCounter(DateTime endTimeSpan, Rate rate)
        {
            DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, 1, 5);
            DateTime currentPayPeriod = firstPayPeriod;
            int payPeriodCounter = 0;
            int daysBetweenIntervals;

            switch (rate)
            {
                case Rate.Weekly:
                    daysBetweenIntervals = 7;
                    break;
                case Rate.Biweekly:
                    daysBetweenIntervals = 14;
                    break;
                case Rate.Monthly:
                    daysBetweenIntervals = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    break;
                case Rate.Yearly:
                    daysBetweenIntervals = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
                    break;
                default:
                    daysBetweenIntervals = 14;
                    break;
            }

            while (currentPayPeriod < DateTime.Now)
            {
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            while (currentPayPeriod < endTimeSpan)
            {
                payPeriodCounter++;
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            return payPeriodCounter;
        }

        //private void ShowBudgetForOtherTimeRange()
        //{

        //    int endTimeSpanDay = Validator.Convert<int>("end day");
        //    int endTimeSpanMonth = Validator.Convert<int>("end month");
        //    int endTimeSpanYear = Validator.Convert<int>("end year");

        //    DateTime endTimeSpan = new DateTime(endTimeSpanYear, endTimeSpanMonth, endTimeSpanDay);

        //    decimal _incomeThisRange = CalculateIncomesForTimePeriod(TimeRange.Other, endTimeSpan);
        //    decimal _expensesThisRange = CalculateExpensesForTimePeriod(TimeRange.Other, endTimeSpan);
        //    decimal _differenceThisRange = _incomeThisRange - _expensesThisRange;

        //    Utilities.PrintMessage($"Expense total for this range: {Utilities.FormatAmount(_expensesThisRange)}", true, true);
        //    Utilities.PrintMessage($"Income total for this range: {Utilities.FormatAmount(_incomeThisRange)}", true, true);
        //    Utilities.PrintMessage($"Your balance for this range is {Utilities.FormatAmount(_differenceThisRange)}", true);
        //}

        //private void ShowBudgetForCurrentMonthAndYear()
        //{
        //    //create budget summary for expenses and incomes this month and year
        //    decimal _incomeThisMonth = CalculateIncomesForTimePeriod(TimeRange.Month);
        //    decimal _expensesThisMonth = CalculateExpensesForTimePeriod(TimeRange.Month);
        //    decimal _incomeThisYear = CalculateIncomesForTimePeriod(TimeRange.Year);
        //    decimal _expensesThisYear = CalculateExpensesForTimePeriod(TimeRange.Year);
        //    decimal _differenceThisMonth = _incomeThisMonth - _expensesThisMonth;
        //    decimal _differenceThisYear = _incomeThisYear - _expensesThisYear;

        //    Utilities.PrintMessage($"Expense total for this month: {Utilities.FormatAmount(_expensesThisMonth)}", true, true);
        //    Utilities.PrintMessage($"Income total for this month: {Utilities.FormatAmount(_incomeThisMonth)}", true, true);
        //    Utilities.PrintMessage($"Expense total for this year: {Utilities.FormatAmount(_expensesThisYear)}", true, true);
        //    Utilities.PrintMessage($"Income total for this year: {Utilities.FormatAmount(_incomeThisYear)}", true, true);
        //    Utilities.PrintMessage($"Your balance for this month is {Utilities.FormatAmount(_differenceThisMonth)}", true);

        //    ConsoleTable currentMonthYearTbl = new ConsoleTable("Expenses This Month", $"{_expensesThisMonth}");
        //    currentMonthYearTbl.AddRow("Incomes This Month", $"{_incomeThisMonth}");
        //    currentMonthYearTbl.AddRow("Balance This Month", $"{_differenceThisMonth}");
        //    currentMonthYearTbl.AddRow("Expenses This Year", $"{_expensesThisYear}");
        //    currentMonthYearTbl.AddRow("Incomes This Year", $"{_incomeThisYear}");
        //    currentMonthYearTbl.AddRow("Balance This Year", $"{_differenceThisYear}");

        //    currentMonthYearTbl.Write();

        //    Utilities.PressEnterToContinue();
        //}

        private void ShowPreviousTransactions()
        {

        }

        #endregion

        #region Instructions
        private void DisplayInstructions()
        {
            Console.WriteLine("Welcome to My Budget App! This app allows you to create your own budget, including keeping track" +
                " of your incomes, expenses, and a wishlist for future purchases. Each menu item should have its own options that" +
                " you can select from. Some menu items even interact with one another (i.e. wishlist feeds into expenses). Finally," +
                " you can update your current balance of money and view your budget summary between incomes, expenses, and your" +
                " wishlist.");
        }
        #endregion

        #region Update Balance
        public void UpdateBalance()
        {
            Console.WriteLine("Please enter your balance");
            //_currentBalance = Validator.Convert<decimal>("current balance");
            //Console.WriteLine($"\nYour current balance is {Utilities.FormatAmount(_currentBalance)}");

        }

        public Rate ProcessRateOption()
        {
            AppScreen.DisplayRateOptions();
            Rate rate = (Validator.Convert<Rate>("a rate"));

            return rate;
        }

        public void Incomes()
        {
            throw new NotImplementedException();
        }

        public void InsertTransaction(long _userAccountId, TransactionType _updateType, decimal _updateAmount, string description)
        {
            throw new NotImplementedException();
        }

        public void ViewTransactions()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods That Need Organizing/Implementing

        //protected bool PreviewUpdate(decimal amount)
        //{
        //    Console.WriteLine("\nSummary");
        //    Console.WriteLine("-------");
        //    Console.WriteLine($"{Utilities.FormatAmount(amount)}\n\n");
        //    Console.WriteLine("");

        //    int opt = Validator.Convert<int>("1 to confirm");
        //    return opt.Equals(1);
        //}

        //public void InsertTransaction(long _userAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        //{
        //    //create a new transaction object
        //    var transaction = new Transaction()
        //    {
        //        TransactionId = Utilities.GetTransactionId(),
        //        UserAccountId = _userAccountId,
        //        TransactionDate = DateTime.Now,
        //        TransactionType = _tranType,
        //        TransactionAmount = _tranAmount,
        //        Description = _desc
        //    };

        //    //add transaction object to the list
        //    _listOfTransactions.Add(transaction);
        //}

        //public void ViewTransactions()
        //{
        //    List<Transaction> filteredTransactionList = _listOfTransactions.Where(t => t.UserAccountId == selectedAccount.Id).ToList();

        //    ConsoleTable consoleTable = new ConsoleTable("Id", "Transaction Date", "Type", "Description", "Amount " + AppScreen.cur);

        //    foreach(Transaction transaction in filteredTransactionList)
        //    {
        //        consoleTable.AddRow(transaction.UserAccountId, transaction.TransactionDate, transaction.TransactionType, transaction.Description, transaction.TransactionAmount);
        //    }

        //    consoleTable.Write();
        //    Utilities.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);

        //}
        #endregion
    }
}

