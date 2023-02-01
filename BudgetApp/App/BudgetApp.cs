using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Entities.Interfaces;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using BudgetApp.Domain;

namespace BudgetApp.App
{
    public class BudgetApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount>? userAccountList;
        protected UserAccount? selectedAccount;
        private List<Transaction>? _listOfTransactions;

        private decimal amountNeededPerMonth;
        private decimal _currentBalance;
        private decimal _estimatedIncome = 9000.00M;
        private decimal _difference;
        private decimal _sumOfAllWishlistExpenses = 0;
        private decimal allIncomes = 0;
        private decimal weeklyIncomes = 0;
        private decimal biweeklyIncomes = 0;
        private decimal biweeklyIncomeThisMonth = 0;
        private decimal _sumOfAllBiweeklyIncomesThisYear = 0;
        private decimal payAtSelectedTime = 0;
        private decimal monthlyIncomes = 0;
        private decimal yearlyIncomes = 0;
        private int _incomeIdCounter = 2;
        private int _wishlistIdCounter = 1;
        public decimal SumOfAllExpenses { get; set; }
        private decimal _sumOfAllMonthlyExpenses = 0;

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserPasscode();
            AppScreen.WelcomeCustomer(selectedAccount!.FullName!);
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
                    ExpenseCategories = new Dictionary<string, decimal>
                    {
                        { "other", 0 },
                        { "rent_utilities", 2000 },
                        {"food_general", 300 },
                        {"gas", 100 },
                        {"subscriptions", 85 }
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
                    TotalIncomes = 0,
                    ExpenseCategories = new Dictionary<string, decimal>
                    {
                        { "other", 0 }
                    }
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
                    TotalIncomes = 0,
                    ExpenseCategories = new Dictionary<string, decimal>
                    {
                        { "other", 0 }
                    }
                }
            };
            #endregion

            _listOfTransactions = new List<Transaction>();
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
            
            _sumOfAllMonthlyExpenses = CalculateTotalExpenses();

            allIncomes = CalculateIncomesForThisMonth() + biweeklyIncomeThisMonth;
            Console.WriteLine(allIncomes);
            amountNeededPerMonth = _sumOfAllMonthlyExpenses;
            _difference = _currentBalance + allIncomes - amountNeededPerMonth;

            AppScreen.DisplayBudgetSummaryOptions();
            ProcessBudgetSummaryMenu();

        }

        private void ProcessBudgetSummaryMenu()
        {
            switch(Validator.Convert<int>("a budget summary option"))
            {
                case 1:
                    ShowBudgetForOtherTimeRange();
                    break;
                case 2:
                    ShowBudgetForCurrentMonthAndYear();
                    break;
                case 3:
                    ShowPreviousTransactions();
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
                    startTimeSpan = DateTime.MinValue;
                    endTimeSpan = DateTime.MinValue;
                    CalculatePay(endTimeSpan);
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

        public decimal CalculatePay(DateTime endTimeSpan)
        {
            DateTime firstPayPeriod = new DateTime(DateTime.Now.Year, 1, 5);
            DateTime currentPayPeriod = firstPayPeriod;
            int payPeriodCounter = 0;

            while (currentPayPeriod < DateTime.Now)
            {
                currentPayPeriod = currentPayPeriod.AddDays(14);
            }

            while (currentPayPeriod < endTimeSpan)
            {
                payPeriodCounter++;
                currentPayPeriod = currentPayPeriod.AddDays(14);
            }

            return 10.00M;
        }

        private void ShowBudgetForOtherTimeRange()
        {
            int otherYear;
            int otherMonth;
            int otherDay;

            Console.WriteLine("Enter the date you'd like to check your budget at");
            
            otherYear = Validator.Convert<int>("year (YYYY)");
            otherMonth = Validator.Convert<int>("month (MM)");
            otherDay = Validator.Convert<int>("day (dd)");

            DateTime otherDate = new DateTime(otherYear, otherMonth, otherDay);
            decimal income = CalculatePay(otherDate);

            Utilities.PrintMessage($"Expense total for this month: {Utilities.FormatAmount(amountNeededPerMonth)}", true, true);
            Utilities.PrintMessage($"Income total through {otherDate.ToString("yyyy, MMMM, dd")}: {Utilities.FormatAmount(income)}", true, true);
            Utilities.PrintMessage($"Your balance for this month is {Utilities.FormatAmount(_difference)}", true);
        }

        private void ShowBudgetForCurrentMonthAndYear()
        {
            decimal _incomeThisMonth = CalculateIncomesForThisMonth();
            decimal _expensesThisMonth = _sumOfAllMonthlyExpenses;
            decimal _differenceThisMonth = _incomeThisMonth - _expensesThisMonth;

            Utilities.PrintMessage($"Expense total for this month: {Utilities.FormatAmount(_expensesThisMonth)}", true, true);
            Utilities.PrintMessage($"Income total for this month: {Utilities.FormatAmount(_incomeThisMonth)}", true, true);
            Utilities.PrintMessage($"Income total for this year: {Utilities.FormatAmount(_sumOfAllBiweeklyIncomesThisYear)}", true, true);
            Utilities.PrintMessage($"Your balance for this month is {Utilities.FormatAmount(_differenceThisMonth)}", true);
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

            string dateDepositedStr = new DateTime().ToString($"MMMM-dd");

            Utilities.PrintMessage($"Your new income details: {dateDepositedStr}", true);

            AppScreen.DisplayRateOptions();
            Rate incomeRate = ProcessIncomeRateOption();
            selectedAccount.IncomeList.Add(new Income { IncomeName = incomeName, Amount = incomeAmount, Rate = incomeRate, Id = _incomeIdCounter });
            Console.WriteLine("New income summary:\n");
            Console.WriteLine($"Income: {incomeName}, Amount: {Utilities.FormatAmount(incomeAmount)}, Frequency/Rate: {incomeRate}, Id: {_incomeIdCounter}");

        }

        private Rate ProcessIncomeRateOption()
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
                    selectedAccount.IncomeList.Find(i => i.Id == id).Amount = Validator.Convert<decimal>("new amount");
                    break;
                case 3:
                    Console.WriteLine("Rate");
                    AppScreen.DisplayRateOptions();
                    selectedAccount.IncomeList.Find(i => i.Id == id).Rate = ProcessIncomeRateOption();
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

        decimal CalculateIncomesForThisMonth()
        {
            allIncomes = 0;
            weeklyIncomes = 0;
            biweeklyIncomes = 0;
            monthlyIncomes = 0;
            yearlyIncomes = 0;

            foreach (Income income in selectedAccount.IncomeList)
            {
                monthlyIncomes += CalculateIncomeByRateAndTime(TimeRange.Month, income);
            }

            CalculateIncomesForEachRate();

            return monthlyIncomes;
        }
        #endregion

        #region Expenses
        public void CategorizedExpenses()
        {
            _sumOfAllMonthlyExpenses = CalculateTotalExpenses();

            Console.WriteLine($"\nSum of monthly expenses: {Utilities.FormatAmount(_sumOfAllMonthlyExpenses)}\n");

            Console.WriteLine($"\nSum of monthly expenses plus wishlist:{Utilities.FormatAmount(_sumOfAllMonthlyExpenses + _sumOfAllWishlistExpenses)}");

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

            foreach (KeyValuePair<string, decimal> expense in selectedAccount.ExpenseCategories)
            {
                _sumOfAllMonthlyExpenses += expense.Value;
            }

            return _sumOfAllMonthlyExpenses;
        }

        private string ChooseMonthlyExpense()
        {
            switch (Validator.Convert<int>("an expense to add"))
            {
                case 1:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("rent_utilities"))
                    {
                        selectedAccount.ExpenseCategories.Add("rent_utilities", 0);
                    }
                    return "rent_utilities";
                case 2:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("credit_cards"))
                    {
                        selectedAccount.ExpenseCategories.Add("credit_cards", 0);
                    }
                    return "credit_cards";
                case 3:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("food_general"))
                    {
                        selectedAccount.ExpenseCategories.Add("food_general", 0);
                    }
                    return "food_general";
                case 4:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("loans"))
                    {
                        selectedAccount.ExpenseCategories.Add("loans", 0);
                    }
                    return "loans";
                case 5:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("gas"))
                    {
                        selectedAccount.ExpenseCategories.Add("gas", 0);
                    }
                    return "gas";
                case 6:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("medical"))
                    {
                        selectedAccount.ExpenseCategories.Add("medical", 0);
                    }
                    return "medical";
                case 7:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("insurance"))
                    {
                        selectedAccount.ExpenseCategories.Add("insurance", 0);
                    }
                    return "insurance";
                case 8:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("subscriptions"))
                    {
                        selectedAccount.ExpenseCategories.Add("subscriptions", 0);
                    }
                    return "subscriptions";
                case 9:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("gym"))
                    {
                        selectedAccount.ExpenseCategories.Add("gym", 0);
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

                    if (!selectedAccount.ExpenseCategories.ContainsKey(newCategory))
                    {
                        selectedAccount.ExpenseCategories.Add(newCategory, 0);
                    }

                    Utilities.PrintMessage($"You have added {newCategory} to your list of expenses", true);

                    Console.WriteLine("Expense categories:\n");

                    foreach (KeyValuePair<string, decimal> expense in selectedAccount.ExpenseCategories)
                    {
                        Console.WriteLine(expense.Key);
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
                                foreach (KeyValuePair<string, decimal> category in selectedAccount.ExpenseCategories)
                                {
                                    Console.WriteLine(category.Key);
                                }

                                newCategory = Utilities.GetUserInput("other category name to be used");

                                if (selectedAccount.ExpenseCategories.ContainsKey(newCategory))
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

            if (!selectedAccount.ExpenseCategories.ContainsKey("other"))
            {
                selectedAccount.ExpenseCategories.Add("other", 0);
            }
            Console.WriteLine("You have chosen other");
            return "other";
        }

        private decimal PayFullExpense(string key)
        {
            decimal payment = selectedAccount.ExpenseCategories[key];
            selectedAccount.ExpenseCategories[key] = 0;
            Utilities.PrintMessage($"You have successfully paid off {key}.", true);
            return payment;
        }

        private void PayPartialExpense(string key, decimal amount)
        {
            decimal newAmount = selectedAccount.ExpenseCategories[key] - amount;

            selectedAccount.ExpenseCategories[key] -= amount;
            Utilities.PrintMessage($"You have successfully paid {amount} towards {key}. Your remaining expense amount is {Utilities.FormatAmount(newAmount)}", true);
        }

        private void UpdateMonthlyExpenseAmount(string key)
        {
            decimal monthlyExpenseAmount = 0;

            monthlyExpenseAmount = Validator.Convert<decimal>("monthly expense amount");

            selectedAccount.ExpenseCategories[key] = monthlyExpenseAmount;

            Console.WriteLine($"The monthly amount for {key} is {Utilities.FormatAmount(selectedAccount.ExpenseCategories[key])}");
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

            InsertTransaction(TransactionType.Expense, expense_amt, "");
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
            Console.WriteLine("This method will allow users to view past expenses/incomes");
        }

        public void InsertTransaction(TransactionType _updateType, decimal _updateAmount, string description)
        {
            throw new NotImplementedException();
        }

        public void ViewTransaction()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

