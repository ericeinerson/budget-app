using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Entities.Interfaces;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using BudgetApp.Domain;
using ConsoleTables;
using System.Reflection.Emit;

namespace BudgetApp.App
{
    public partial class BudgetApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount>? userAccountList;
        protected UserAccount selectedAccount = new UserAccount();
        private List<Transaction> _listOfTransactions = new List<Transaction>();

        private decimal _currentBalance;
        private decimal _sumOfAllWishlistExpenses = 0;
        private decimal _weeklyExpenses = 0;
        private decimal _biweeklyExpenses = 0;
        private decimal _monthlyExpenses = 0;
        private decimal _yearlyExpenses = 0;
        private decimal allIncomes = 0;
        private decimal weeklyIncomes = 0;
        private decimal biweeklyIncomes = 0;
        private decimal monthlyIncomes = 0;
        private decimal yearlyIncomes = 0;
        private int _incomeIdCounter;
        private int _wishlistIdCounter = 1;
        public decimal SumOfAllExpenses { get; set; }
        private decimal _sumOfAllMonthlyExpenses = 0;
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
                TotalExpenses = 0,
                TotalIncomes = 0,
                ExpenseList = new List<Expense>(),
                Wishlist = new Wishlist(),
                IncomeList = new List<Income>()
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

            _listOfTransactions = new List<Transaction>();
            _incomeIdCounter = selectedAccount.IncomeList.Count;
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
                    ManageIncome();
                    break;
                case (int)AppMenu.CategorizedExpenses:
                    CategorizedExpenses();
                    break;
                case (int)AppMenu.Wishlist:
                    ManageWishList();
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
                    //Utilities.SaveUserInformation(selectedAccount);
                    Utilities.PressEnterToContinue();
                    break;
                case (int)AppMenu.LoadInfo:
                    //selectedAccount = Utilities.LoadUserInformation(selectedAccount);
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

        private void ProcessBudgetSummaryMenu()
        {
            switch(Validator.Convert<int>("a budget summary option"))
            {
                case 1:
                    ShowBudgetForCurrentMonthAndYear();
                    break;
                case 2:
                    ShowBudgetForOtherTimeRange();
                    break;
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

        public decimal CalculateIncomeByRateAndTime(TimeRange timeRange,Income income)
        {
            DateTime currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
            DateTime startTimeSpan;
            DateTime endTimeSpan;

            DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, income.Month, income.Day);
            DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddHours(23);

            DateTime currentPayPeriod = firstPayPeriod;
            int payPeriodCounter = 0;
            int daysBetweenIntervals = 0;
            decimal pay = income.Amount;

            switch (income.Rate)
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
                    daysBetweenIntervals = DateTime.IsLeapYear(DateTime.Now.Year) ? 366: 365;
                    break;
            }
            switch (timeRange)
            {
                case TimeRange.Month:
                    startTimeSpan = currentMonthStart.AddMilliseconds(-1);
                    endTimeSpan = currentMonthEnd;
                    break;
                case TimeRange.Year:
                    startTimeSpan = currentYearStart.AddMilliseconds(-1);
                    endTimeSpan = currentYearEnd;
                    break;
                case TimeRange.Other:
                    startTimeSpan = DateTime.Now;
                    int endTimeSpanDay = Validator.Convert<int>("end day");
                    int endTimeSpanMonth = Validator.Convert<int>("end month");
                    int endTimeSpanYear = Validator.Convert<int>("end year");
                    endTimeSpan = new DateTime(endTimeSpanYear, endTimeSpanMonth, endTimeSpanDay);
                    break;
                default:
                    startTimeSpan = DateTime.MaxValue;
                    endTimeSpan = DateTime.MinValue;
                    break;
            }

            while(currentPayPeriod < startTimeSpan)
            {
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            while (currentPayPeriod < endTimeSpan)
            {
                payPeriodCounter++;
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            pay *= payPeriodCounter;
            return pay;
        }

        public decimal CalculateIncomeByRateAndTime(TimeRange timeRange, Income income, DateTime endTime)
        {
            DateTime currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
            DateTime startTimeSpan;
            DateTime endTimeSpan;

            DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, income.Month, income.Day);
            DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddHours(23);

            DateTime currentPayPeriod = firstPayPeriod;
            int payPeriodCounter = 0;
            int daysBetweenIntervals = 0;
            decimal pay = income.Amount;

            switch (income.Rate)
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
            }
            
            startTimeSpan = DateTime.Now;
            endTimeSpan = endTime;

            while (currentPayPeriod < startTimeSpan)
            {
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            while (currentPayPeriod < endTimeSpan)
            {
                payPeriodCounter++;
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            pay *= payPeriodCounter;
            return pay;
        }

        public decimal CalculateExpenseByRateAndTime(TimeRange timeRange, Expense expense, DateTime endTime)
        {
            DateTime currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
            DateTime startTimeSpan;
            DateTime endTimeSpan;

            DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, expense.Month, expense.Day);
            DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddHours(23);

            DateTime currentPayPeriod = firstPayPeriod;
            int payPeriodCounter = 0;
            int daysBetweenIntervals = 0;
            decimal pay = expense.Amount;

            switch (expense.Rate)
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
            }

            startTimeSpan = DateTime.Now;
            endTimeSpan = endTime;

            while (currentPayPeriod < startTimeSpan)
            {
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            while (currentPayPeriod < endTimeSpan)
            {
                payPeriodCounter++;
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            pay *= payPeriodCounter;
            return pay;
        }

        public decimal CalculateExpenseByRateAndTime(TimeRange timeRange, Expense expense)
        {
            DateTime currentYearStart = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime currentYearEnd = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
            DateTime startTimeSpan;
            DateTime endTimeSpan;

            DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, expense.Month, expense.Day);
            DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime currentMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).AddHours(23);

            DateTime currentPayPeriod = firstPayPeriod;
            int payPeriodCounter = 0;
            int daysBetweenIntervals = 0;
            decimal pay = expense.Amount;

            switch (expense.Rate)
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
            }
            switch (timeRange)
            {
                case TimeRange.Month:
                    startTimeSpan = currentMonthStart.AddMilliseconds(-1);
                    endTimeSpan = currentMonthEnd;
                    break;
                case TimeRange.Year:
                    startTimeSpan = currentYearStart.AddMilliseconds(-1);
                    endTimeSpan = currentYearEnd;
                    break;
                case TimeRange.Other:
                    startTimeSpan = DateTime.Now;
                    int endTimeSpanDay = Validator.Convert<int>("end day");
                    int endTimeSpanMonth = Validator.Convert<int>("end month");
                    int endTimeSpanYear = Validator.Convert<int>("end year");
                    endTimeSpan = new DateTime(endTimeSpanYear, endTimeSpanMonth, endTimeSpanDay);
                    break;
                default:
                    startTimeSpan = DateTime.MaxValue;
                    endTimeSpan = DateTime.MinValue;
                    break;
            }

            while (currentPayPeriod < startTimeSpan)
            {
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            while (currentPayPeriod < endTimeSpan)
            {
                payPeriodCounter++;
                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
            }

            pay *= payPeriodCounter;
            return pay;
        }

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

        private void ShowBudgetForOtherTimeRange()
        {

            int endTimeSpanDay = Validator.Convert<int>("end day");
            int endTimeSpanMonth = Validator.Convert<int>("end month");
            int endTimeSpanYear = Validator.Convert<int>("end year");

            DateTime endTimeSpan = new DateTime(endTimeSpanYear, endTimeSpanMonth, endTimeSpanDay);

            decimal _incomeThisRange = CalculateIncomesForTimePeriod(TimeRange.Other, endTimeSpan);
            decimal _expensesThisRange = CalculateExpensesForTimePeriod(TimeRange.Other, endTimeSpan);
            decimal _differenceThisRange = _incomeThisRange - _expensesThisRange;

            Utilities.PrintMessage($"Expense total for this range: {Utilities.FormatAmount(_expensesThisRange)}", true, true);
            Utilities.PrintMessage($"Income total for this range: {Utilities.FormatAmount(_incomeThisRange)}", true, true);
            Utilities.PrintMessage($"Your balance for this range is {Utilities.FormatAmount(_differenceThisRange)}", true);
        }

        private void ShowBudgetForCurrentMonthAndYear()
        {
            //create budget summary for expenses and incomes this month and year
            decimal _incomeThisMonth = CalculateIncomesForTimePeriod(TimeRange.Month);
            decimal _expensesThisMonth = CalculateExpensesForTimePeriod(TimeRange.Month);
            decimal _incomeThisYear = CalculateIncomesForTimePeriod(TimeRange.Year);
            decimal _expensesThisYear = CalculateExpensesForTimePeriod(TimeRange.Year);
            decimal _differenceThisMonth = _incomeThisMonth - _expensesThisMonth;
            decimal _differenceThisYear = _incomeThisYear - _expensesThisYear;

            Utilities.PrintMessage($"Expense total for this month: {Utilities.FormatAmount(_expensesThisMonth)}", true, true);
            Utilities.PrintMessage($"Income total for this month: {Utilities.FormatAmount(_incomeThisMonth)}", true, true);
            Utilities.PrintMessage($"Expense total for this year: {Utilities.FormatAmount(_expensesThisYear)}", true, true);
            Utilities.PrintMessage($"Income total for this year: {Utilities.FormatAmount(_incomeThisYear)}", true, true);
            Utilities.PrintMessage($"Your balance for this month is {Utilities.FormatAmount(_differenceThisMonth)}", true);

            ConsoleTable currentMonthYearTbl = new ConsoleTable("Expenses This Month", $"{_expensesThisMonth}");
            currentMonthYearTbl.AddRow("Incomes This Month", $"{_incomeThisMonth}");
            currentMonthYearTbl.AddRow("Balance This Month", $"{_differenceThisMonth}");
            currentMonthYearTbl.AddRow("Expenses This Year", $"{_expensesThisYear}");
            currentMonthYearTbl.AddRow("Incomes This Year", $"{_incomeThisYear}");
            currentMonthYearTbl.AddRow("Balance This Year", $"{_differenceThisYear}");

            currentMonthYearTbl.Write();

            Utilities.PressEnterToContinue();
        }

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
            _currentBalance = Validator.Convert<decimal>("current balance");
            Console.WriteLine($"\nYour current balance is {Utilities.FormatAmount(_currentBalance)}");

        }
        #endregion

        #region Methods That Need Organizing/Implementing
        public void Incomes()
        {
            Console.WriteLine("will implement this later");
        }

        protected bool PreviewUpdate(decimal amount)
        {
            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{Utilities.FormatAmount(amount)}\n\n");
            Console.WriteLine("");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _userAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            //create a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utilities.GetTransactionId(),
                UserAccountId = _userAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc
            };

            //add transaction object to the list
            _listOfTransactions.Add(transaction);
        }

        public void ViewTransactions()
        {
            List<Transaction> filteredTransactionList = _listOfTransactions.Where(t => t.UserAccountId == selectedAccount.Id).ToList();

            ConsoleTable consoleTable = new ConsoleTable("Id", "Transaction Date", "Type", "Description", "Amount " + AppScreen.cur);
            
            foreach(Transaction transaction in filteredTransactionList)
            {
                consoleTable.AddRow(transaction.UserAccountId, transaction.TransactionDate, transaction.TransactionType, transaction.Description, transaction.TransactionAmount);
            }

            consoleTable.Write();
            Utilities.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);

        }
        #endregion
    }
}

