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
    public class BudgetApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount>? userAccountList;
        protected UserAccount selectedAccount = new UserAccount();
        private List<Transaction>? _listOfTransactions;

        private decimal amountNeededPerMonth;
        private decimal _currentBalance;
        private decimal _estimatedIncome = 9000.00M;
        private decimal _difference;
        private decimal _sumOfAllWishlistExpenses = 0;
        private decimal _weeklyExpenses = 0;
        private decimal _biweeklyExpenses = 0;
        private decimal _monthlyExpenses = 0;
        private decimal _yearlyExpenses = 0;
        private decimal allIncomes = 0;
        private decimal weeklyIncomes = 0;
        private decimal biweeklyIncomes = 0;
        private decimal biweeklyIncomeThisMonth = 0;
        private decimal _sumOfAllBiweeklyIncomesThisYear = 0;
        private decimal payAtSelectedTime = 0;
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
            ProcessAppMenuOption();
        }

        #region Initialize Data
        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = 1,
                    FullName = "Eric Einerson",
                    Passcode = 0991,
                    Balance = 0,
                    IsLocked = false,
                    TotalLogin = 0,
                    TotalExpenses = 0,
                    TotalIncomes = 0,
                    ExpenseList = new List<Expense>
                    {
                        new Expense
                        {
                            ExpenseName = "rent_utilities",
                            Amount = 2000.00M,
                            Day = 1,
                            Month = 1,
                            Rate = Rate.Monthly,
                            Id = 1
                        },
                        new Expense
                        {
                            ExpenseName = "credit_cards",
                            Amount = 400.00M,
                            Day = 3,
                            Month = 1,
                            Rate = Rate.Monthly,
                            Id = 1
                        },
                        new Expense
                        {
                            ExpenseName = "gas",
                            Amount = 125.00M,
                            Day = 1,
                            Month = 1,
                            Rate = Rate.Monthly,
                            Id = 1
                        },
                        new Expense
                        {
                            ExpenseName = "subscriptions",
                            Amount = 100.00M,
                            Day = 10,
                            Month = 1,
                            Rate = Rate.Monthly,
                            Id = 1
                        }
                    },
                    Wishlist = new Wishlist
                    {
                        Items = new List<WishlistItem>
                        {
                            new WishlistItem
                            {
                                Item = "Breath of the Wild",
                                Cost = 60.00M,
                                Id = 1,
                                Priority = 1
                            },
                            new WishlistItem
                            {
                                Item = "Betrayal: Legacy",
                                Cost = 100.00M,
                                Id = 2,
                                Priority = 2
                            }
                        }
                    },
                    IncomeList = new List<Income>
                    {
                        new Income
                        {
                            IncomeName = "paycheck",
                            Amount = 1000.00M,
                            Rate = Rate.Biweekly,
                            Day = 5,
                            Month = 1,
                            Id = 1
                        },
                        new Income
                        {
                            IncomeName = "taxes",
                            Amount = 1000.00M,
                            Rate = Rate.Yearly,
                            Id = 2,
                            Day = 12,
                            Month = 11
                        },
                        new Income
                        {
                            IncomeName = "freelance",
                            Amount = 30.00M,
                            Rate = Rate.Weekly,
                            Day = 2,
                            Month = 1,
                            Id = 3
                        },
                        new Income
                        {
                            IncomeName = "mow lawn",
                            Amount = 10.00M,
                            Rate = Rate.Weekly,
                            Day = 6,
                            Month = 1,
                            Id = 4
                        }
                    }

                },
                new UserAccount
                {
                    Id = 2,
                    FullName = "Pickle Rick",
                    Passcode = 8374,
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
                    Passcode = 1111,
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

        #region Income
        public void ManageIncome()
        {
            CalculateIncomesForEachRate();

            Console.WriteLine($"\nSum of weekly incomes: {Utilities.FormatAmount(weeklyIncomes)}\n");
            Console.WriteLine($"\nSum of biweekly incomes: {Utilities.FormatAmount(biweeklyIncomes)}\n");
            Console.WriteLine($"\nSum of monthly incomes: {Utilities.FormatAmount(monthlyIncomes)}\n");
            Console.WriteLine($"\nSum of yearly incomes: {Utilities.FormatAmount(yearlyIncomes)}\n");

            Utilities.PressEnterToContinue();
            AppScreen.DisplayIncomeMenu();

            ProcessIncomeUpdateOption();

            Utilities.PressEnterToContinue();
        }

        void CalculateIncomesForEachRate()
        {
            weeklyIncomes = 0;
            biweeklyIncomes = 0;
            monthlyIncomes = 0;
            yearlyIncomes = 0;

            foreach (Income income in selectedAccount.IncomeList)
            {
                switch (income.Rate)
                {
                    case Rate.Weekly:
                        weeklyIncomes += income.Amount;
                        break;
                    case Rate.Biweekly:
                        biweeklyIncomes += income.Amount;
                        break;
                    case Rate.Monthly:
                        monthlyIncomes += income.Amount;
                        break;
                    case Rate.Yearly:
                        yearlyIncomes += income.Amount;
                        break;
                }
            }
        }

        private void ProcessIncomeUpdateOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    Console.WriteLine("View Incomes");
                    ViewIncome();
                    break;
                case 2:
                    Console.WriteLine("Add new income");
                    AddNewIncome();
                    break;
                case 3:
                    Console.WriteLine("Update an income\n\n");
                    UpdateIncome();
                    break;
                case 4:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessIncomeUpdateOption();
                    break;
            }
        }

        private void ViewIncome()
        {
            foreach (Income i in selectedAccount.IncomeList)
            {
                Console.WriteLine($"Income: {i.IncomeName}, Amount: {Utilities.FormatAmount(i.Amount)}, Frequency/Rate: {i.Rate}, Id: {i.Id}");
            }
            Utilities.PressEnterToContinue();
        }

        private void AddNewIncome()
        {
            _incomeIdCounter++;

            string incomeName = Utilities.GetUserInput("income name");
            decimal incomeAmount = Validator.Convert<decimal>("income amount");
            int monthDeposited = Validator.Convert<int>("month of each deposit");
            int dayDeposited = Validator.Convert<int>("day of each deposit");
            DateTime dateDeposited = new DateTime(DateTime.Now.Year, monthDeposited, dayDeposited);
            AppScreen.DisplayRateOptions();
            Rate incomeRate = ProcessRateOption();

            selectedAccount.IncomeList.Add(new Income { IncomeName = incomeName, Amount = incomeAmount, Day = dayDeposited, Month = monthDeposited, Rate = incomeRate, Id = _incomeIdCounter });

            ConsoleTable newIncomeTbl = new ConsoleTable("Name", "Amount", "Start Date", "Rate Of Deposit");
            newIncomeTbl.AddRow(incomeName, incomeAmount, dateDeposited.ToString("MMMM dd"), incomeRate);
            newIncomeTbl.Options.EnableCount = false;
            newIncomeTbl.Write();

            InsertTransaction(selectedAccount.Id, TransactionType.Income, incomeAmount, $"added new income {incomeName} to list of incomes");
        }

        private Rate ProcessRateOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    Console.WriteLine("Weekly");
                    return Rate.Weekly;
                case 2:
                    Console.WriteLine("Biweekly");
                    return Rate.Biweekly;
                case 3:
                    Console.WriteLine("Monthly");
                    return Rate.Monthly;
                case 4:
                    Console.WriteLine("Yearly");
                    return Rate.Yearly;
                case 5:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    return Rate.Other;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessIncomeUpdateOption();
                    return Rate.Other;
            }
        }

        private void UpdateIncome()
        {
            int selectedIncomeId = Validator.Convert<int>("Enter Id of income to update");
            try
            {
                selectedAccount.IncomeList.Find(i => i.Id == selectedIncomeId);
            }
            catch
            {
                Utilities.PrintMessage("Id could not be found. Please try again.", false);
                UpdateIncome();
            }
            AppScreen.DisplayIncomeUpdateOptions();
            ProcessIncomeChangeOption(selectedIncomeId);
        }

        private void ProcessIncomeChangeOption(int id)
        {
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    Console.WriteLine("Name");
                    selectedAccount.IncomeList.Find(i => i.Id == id).IncomeName = Utilities.GetUserInput("new name");
                    break;
                case 2:
                    Console.WriteLine("Amount");
                    Income selectedIncome = selectedAccount.IncomeList.Find(i => i.Id == id);
                    selectedIncome.Amount = Validator.Convert<decimal>("new amount");
                    InsertTransaction(selectedAccount.Id, TransactionType.Income, selectedIncome.Amount, $"updated amount of income {selectedIncome.IncomeName}");
                    break;
                case 3:
                    Console.WriteLine("Rate");
                    AppScreen.DisplayRateOptions();
                    selectedAccount.IncomeList.Find(i => i.Id == id).Rate = ProcessRateOption();
                    break;
                case 4:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessIncomeUpdateOption();
                    break;
            }

        }

        decimal CalculateIncomesForTimePeriod(TimeRange timeRange, DateTime endTime)
        {
            allIncomes = 0;
            
            foreach (Income income in selectedAccount.IncomeList)
            {
                allIncomes += CalculateIncomeByRateAndTime(timeRange, income, endTime);
            }

            CalculateIncomesForEachRate();

            return allIncomes;
        }

        decimal CalculateExpensesForTimePeriod(TimeRange timeRange, DateTime endTime)
        {
            
            SumOfAllExpenses = 0;

            foreach (Expense expense in selectedAccount.ExpenseList)
            {
                SumOfAllExpenses += CalculateExpenseByRateAndTime(timeRange, expense, endTime);
            }

            CalculateExpensesForEachRate();

            return SumOfAllExpenses;
        }

        decimal CalculateIncomesForTimePeriod(TimeRange timeRange)
        {
            allIncomes = 0;

            foreach (Income income in selectedAccount.IncomeList)
            {
                allIncomes += CalculateIncomeByRateAndTime(timeRange, income);
            }

            CalculateIncomesForEachRate();

            return allIncomes;
        }

        decimal CalculateExpensesForTimePeriod(TimeRange timeRange)
        {

            SumOfAllExpenses = 0;

            foreach (Expense expense in selectedAccount.ExpenseList)
            {
                SumOfAllExpenses += CalculateExpenseByRateAndTime(timeRange, expense);
            }

            CalculateExpensesForEachRate();

            return SumOfAllExpenses;
        }


        #endregion

        #region Expenses
        public void CategorizedExpenses()
        {
            CalculateExpensesForEachRate();

            Console.WriteLine($"\nSum of weekly expenses: {Utilities.FormatAmount(_weeklyExpenses)}\n");
            Console.WriteLine($"\nSum of biweekly expenses: {Utilities.FormatAmount(_biweeklyExpenses)}\n");
            Console.WriteLine($"\nSum of monthly expenses: {Utilities.FormatAmount(_monthlyExpenses)}\n");
            Console.WriteLine($"\nSum of yearly expenses: {Utilities.FormatAmount(_yearlyExpenses)}\n");

            Utilities.PressEnterToContinue();
            AppScreen.DisplayExpenseOptions();

            string chosenExpense = ChooseMonthlyExpense();

            AppScreen.DisplayExpenseUpdateOptions();

            int expenseUpdateOption = ProcessExpenseUpdateOption();


            switch (expenseUpdateOption)
            {
                case 1:
                    decimal payOff = PayFullExpense(chosenExpense);
                    ProcessExpense(payOff);
                    break;
                case 2:
                    var expense_amt = Validator.Convert<decimal>("expense amount");
                    PayPartialExpense(chosenExpense, expense_amt);
                    ProcessExpense(expense_amt);
                    break;
                case 3:
                    UpdateMonthlyExpenseAmount(chosenExpense);
                    break;
                default:
                    ProcessExpenseUpdateOption();
                    break;
            }

            Utilities.PressEnterToContinue();
        }

        private decimal CalculateTotalExpenses()
        {
            _sumOfAllMonthlyExpenses = 0;

            foreach (Expense expense in selectedAccount.ExpenseList)
            {
                _sumOfAllMonthlyExpenses += expense.Amount;
            }

            return _sumOfAllMonthlyExpenses;
        }

        public string ChooseMonthlyExpense()
        {

            switch (Validator.Convert<int>("an expense to update or add"))
            {
                case 1:
                    if (!selectedAccount.ExpenseList.Any(e=>e.ExpenseName == "rent_utilities"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("rent_utilities"));
                    }
                    return "rent_utilities";
                case 2:
                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "credit_cards"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("credit_cards"));
                    }
                    return "credit_cards";
                case 3:
                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "food_general"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("food_general"));
                    }
                    return "food_general";
                case 4:
                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "loans"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("loans"));
                    }
                    return "loans";
                case 5:
                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "gas"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("gas"));
                    }
                    return "gas";
                case 6:
                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "medical"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("medical"));
                    }
                    return "medical";
                case 7:
                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "insurance"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("insurance"));
                    }
                    return "insurance";
                case 8:
                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "subscriptions"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("subscriptions"));
                    }
                    return "subscriptions";
                case 9:
                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == "gym"))
                    {
                        Utilities.PrintMessage("Expense not added to list. Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense("gym"));
                    }
                    return "gym";
                case 10:
                    return ProcessOtherExpense();

                case 11:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    return string.Empty;
                case 12:
                    AppScreen.DisplayAppMenu();
                    ProcessAppMenuOption();
                    return string.Empty;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ChooseMonthlyExpense();
                    return string.Empty;
            }
        }

        public Expense ConstructExpense(string expenseName = "")
        {
            Expense expense = new Expense();
            string name = expenseName;
            decimal amount = Validator.Convert<decimal>("expense amount");
            int id = expenseId;
            expenseId++;
            int day = Validator.Convert<int>("day expense is withdrawn");
            int month = Validator.Convert<int>("month expense is withdrawn. Enter 1 as default if this is a monthly expense");
            AppScreen.DisplayRateOptions();
            Rate rate = ProcessRateOption();

            expense.ExpenseName = name;
            expense.Amount = amount;
            expense.Id = id;
            expense.Day = day;
            expense.Month = month;
            expense.Rate = rate;

            return expense;
        }

        private int ProcessExpenseUpdateOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    Console.WriteLine("Pay All/Remaining");
                    return 1;
                case 2:
                    Console.WriteLine("Pay Partial");
                    return 2;
                case 3:
                    Console.WriteLine("You have chosen to update an expense\n\n");
                    return 3;
                case 4:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    return 4;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessExpenseUpdateOption();
                    return 0;
            }
        }

        public string ProcessOtherExpense()
        {
            Console.WriteLine("Would you like to add a new category (Y/N)?\n");
            string otherCategory = string.Empty;
            string newCategory = string.Empty;

            while (true)
            {
                otherCategory = Console.ReadLine();
                if (otherCategory == "Y")
                {
                    newCategory = Utilities.GetUserInput("new category");

                    if (!selectedAccount.ExpenseList.Any(e => e.ExpenseName == newCategory))
                    {
                        Utilities.PrintMessage("Enter the following information to add to list:", true);

                        selectedAccount.ExpenseList.Add(ConstructExpense(newCategory));
                    }

                    Utilities.PrintMessage($"You have added {newCategory} to your list of expenses", true);

                    Console.WriteLine("Expense categories:\n");

                    foreach (Expense expense in selectedAccount.ExpenseList)
                    {
                        Console.WriteLine(expense.ExpenseName);
                    }
                    return newCategory;
                }
                else if (otherCategory == "N")
                {
                    Console.WriteLine("Would you like to use an existing other category (Y/N)?\n");

                    while (true)
                    {
                        otherCategory = Console.ReadLine();

                        if (otherCategory == "Y")
                        {
                            while (true)
                            {
                                Console.WriteLine("Choose from the categories below:\n");
                                foreach (Expense category in selectedAccount.ExpenseList)
                                {
                                    Console.WriteLine(category.ExpenseName);
                                }

                                newCategory = Utilities.GetUserInput("other category name to be used");

                                if (selectedAccount.ExpenseList.Any(e=>e.ExpenseName == newCategory))
                                {
                                    return newCategory;
                                }
                                else if (newCategory == "q")
                                {
                                    AppScreen.LogoutProgress();
                                    Utilities.PrintMessage("You have successfully logged out.", true);
                                    Run();
                                }
                                else
                                {
                                    Utilities.PrintMessage("Invalid input. Try again or press q to logout", false);
                                    continue;
                                }
                            }

                        }
                        else if (otherCategory == "N")
                        {
                            break;
                        }
                        else
                        {
                            Utilities.PrintMessage("Invalid input. Try again", false, true);
                            Console.WriteLine("Enter Y/N");

                            continue;
                        }
                    }
                    break;
                }
                else
                {
                    Utilities.PrintMessage("Invalid input. Please try again", false, true);
                    Console.WriteLine("Enter Y/N");
                    continue;
                }
            }

            if (!selectedAccount.ExpenseList.Any(e=>e.ExpenseName == "other"))
            {
                selectedAccount.ExpenseList.Add(ConstructExpense("other"));
            }
            Console.WriteLine("You have chosen other");
            return "other";
        }

        private decimal PayFullExpense(string expenseName)
        {
            Expense expense = selectedAccount.ExpenseList.Find(e => e.ExpenseName == expenseName);
            decimal payment = expense.Amount;
            expense.Amount = 0;
            Utilities.PrintMessage($"You have successfully paid off {expenseName}.", true);
            return payment;
        }

        private void PayPartialExpense(string expenseName, decimal payment)
        {
            Expense expense = selectedAccount.ExpenseList.Find(e => e.ExpenseName == expenseName);
            decimal newAmount = expense.Amount - payment;

            expense.Amount -= payment;
            Utilities.PrintMessage($"You have successfully paid {payment} towards {expenseName}. Your remaining expense amount is {Utilities.FormatAmount(newAmount)}", true);
        }

        private void UpdateMonthlyExpenseAmount(string expenseName)
        {
            Expense expense = selectedAccount.ExpenseList.Find(e => e.ExpenseName == expenseName);

            decimal monthlyExpenseAmount = 0;

            monthlyExpenseAmount = Validator.Convert<decimal>("monthly expense amount");

            expense.Amount = monthlyExpenseAmount;

            Console.WriteLine($"The monthly amount for {expenseName} is {Utilities.FormatAmount(monthlyExpenseAmount)}");

            InsertTransaction(selectedAccount.Id, TransactionType.Expense, monthlyExpenseAmount, $"updated expense amount for {expenseName}");
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

            InsertTransaction(selectedAccount.Id, TransactionType.Expense, expense_amt, $"paid off expense in full");
        }
        void CalculateExpensesForEachRate()
        {
            _weeklyExpenses = 0;
            _biweeklyExpenses = 0;
            _monthlyExpenses = 0;
            _yearlyExpenses = 0;

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
        #endregion

        #region Wishlist
        public void ManageWishList()
        {
            AppScreen.DisplayWishlistOptions();
            ProcessWishlistOption();
        }

        public void ProcessWishlistOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    Console.WriteLine("View Wishlist");
                    foreach (WishlistItem item in selectedAccount.Wishlist.Items)
                    {
                        Console.WriteLine($"Item: {item.Item}, Cost: {Utilities.FormatAmount(item.Cost)}, Priority: {item.Priority}, Id: {item.Id}\n");
                    }
                    Utilities.PressEnterToContinue();
                    break;
                case 2:
                    Console.WriteLine("Add Wishlist Item");
                    _wishlistIdCounter++;
                    string newItemName = Utilities.GetUserInput("item name");
                    decimal newItemCost = Validator.Convert<decimal>("item cost");
                    int newItemPriority = Validator.Convert<int>("item priority");
                    foreach (WishlistItem item in selectedAccount.Wishlist.Items)
                    {
                        if (newItemPriority <= item.Priority)
                        {
                            item.Priority++;
                            Utilities.PrintMessage($"{item.Item}'s new priority is {item.Priority}", true);
                        }
                    }
                    selectedAccount.Wishlist.Items.Add(new WishlistItem { Item = newItemName, Cost = newItemCost, Priority = newItemPriority, Id = _wishlistIdCounter });

                    break;
                case 3:
                    Console.WriteLine("Pay For Wishlist Item");
                    foreach (WishlistItem item in selectedAccount.Wishlist.Items)
                    {
                        Console.WriteLine($"Item: {item.Item}, Cost: {Utilities.FormatAmount(item.Cost)}, Priority: {item.Priority}, Id: {item.Id}\n");
                    }

                    int wishListItemId = Validator.Convert<int>("wishlist id");
                    try
                    {
                        WishlistItem selectedItem = selectedAccount.Wishlist.Items.Find(item => item.Id == wishListItemId);
                        _sumOfAllWishlistExpenses += selectedItem.Cost;
                        foreach (WishlistItem item in selectedAccount.Wishlist.Items)
                        {
                            if (item.Priority > selectedItem.Priority)
                            {
                                item.Priority--;
                            }
                        }
                        selectedAccount.Wishlist.Items.Remove(selectedItem);
                        Utilities.PrintMessage($"Success. Your new wishlist balance is {Utilities.FormatAmount(_sumOfAllWishlistExpenses)}", true);
                    }
                    catch
                    {
                        Utilities.PrintMessage("Invalid input. Please try again.", false);
                        ProcessWishlistOption();
                    }

                    break;
                case 4:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessWishlistOption();
                    break;
            }
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

