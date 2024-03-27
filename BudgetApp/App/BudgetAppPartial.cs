using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Entities.Interfaces;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;

namespace BudgetApp.App
{
    public partial class BudgetApp : IUserLogin, ITransaction
    {
        private List<UserAccount>? userAccountList;
        protected UserAccount selectedAccount = new();
        
        public void Run()
        {
            AppScreen.Welcome();
            CheckUserPasscode();
            AppScreen.WelcomeCustomer(selectedAccount.FullName!);
            bool isSavedData = Utilities.CheckForExistingUserFile(selectedAccount);
            if (isSavedData)
            {
                var loadData = PromptUserToLoadData();
                if (loadData)
                {
                    ValidateTransactionsForTimePeriod();
                }
                Utilities.SaveUserInfoOnlyWithNewLoginTime(selectedAccount);
            }
            else
            {
                Utilities.PrintMessage("No save data found", true, false);
            }
            VerifyTransactionStatus();
            AppScreen.DisplayAppMenu();
            ProcessAppMenuOption();
        }

        #region Initialize Data
        public void InitializeData()
        {
            UserAccount self = new()
            {
                Id = 1,
                Directory = "",
                FullName = "e",
                Passcode = "0000",
                Balance = 0,
                IsLocked = false,
                TotalLogin = 0,
                BudgetItemIdCounter = 6,
                ExpenseList = new List<BudgetItem>() {
                    new Expense() {
                        Amount = 111,
                        Name = "expense1",
                        Rate = Rate.NoRate,
                        Id = 1,
                        StartDate = new DateTime(2020, 12, 15),
                        CategoryId = 2,
                    },
                    new Expense() {
                        Amount = 112,
                        Name = "expense2",
                        Rate = Rate.Biweekly,
                        Id = 2,
                        StartDate = new DateTime(2018, 1, 31),
                        CategoryId = 1,
                    },
                    new Expense() {
                        Amount = 10,
                        Name = "expense3",
                        Rate = Rate.Monthly,
                        Id = 3,
                        StartDate = new DateTime(1999, 4, 30),
                        EndDate = new DateTime(2001, 1, 31),
                        CategoryId = 0,
                    },
                    new Expense()
                    {
                        Amount = 345,
                        Name = "expense4.3.1415",
                        Rate = Rate.Weekly,
                        Id = 4,
                        StartDate = new DateTime(1952, 4, 30),
                        CategoryId = 1,
                    },
                    new Expense() {
                        Amount = 777,
                        Name = "expense7",
                        Rate = Rate.Monthly,
                        Id = 7,
                        StartDate = new DateTime(2023, 12, 15),
                        EndDate = new DateTime(2025, 12, 14),
                        CategoryId = 0,
                    },
                    new Expense() {
                        Amount = 88,
                        Name = "expense8",
                        Rate = Rate.Biweekly,
                        Id = 8,
                        StartDate = new DateTime(2022, 12, 15),
                        EndDate = new DateTime(2024, 1, 1),
                        CategoryId = 2,
                    },
                },
                IncomeList = new List<BudgetItem>() {
                    new Income() {
                        Amount = 111,
                        Name = "income1",
                        Rate = Rate.Yearly,
                        Id = 5,
                        StartDate = new DateTime(2021, 11, 11),
                        CategoryId = 2,
                    },
                    new Income() {
                        Amount = 112,
                        Name = "income22",
                        Rate = Rate.NoRate,
                        Id = 6,
                        StartDate = new DateTime(2000, 2, 29),
                        CategoryId = 1,
                    },
                    new Income() {
                        Amount = 112,
                        Name = "income3",
                        Rate = Rate.Monthly,
                        Id = -2,
                        StartDate = new DateTime(199, 1, 30),
                        CategoryId = 0,
                        EndDate = new DateTime(200, 2, 28)
                    },
                    new Income() {
                        Amount = 999,
                        Name = "income9",
                        Rate = Rate.Monthly,
                        Id = 9,
                        StartDate = new DateTime(2023, 12, 15),
                        EndDate = new DateTime(2025, 12, 14),
                        CategoryId = 0,
                    },
                    new Income() {
                        Amount = 10.10M,
                        Name = "income10",
                        Rate = Rate.Yearly,
                        Id = 10,
                        StartDate = new DateTime(2000, 12, 15),
                        EndDate = new DateTime(2026, 1, 1),
                        CategoryId = 1,
                    },
                },
                TransactionList = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Name = "income3",
                        Amount = 112,
                        BudgetItemId = -2,
                        CategoryId = 1,
                        ScheduledDate = new DateTime(199, 3, 30)
                    },
                    new Transaction()
                    {
                        Name = "income3",
                        Amount = 112,
                        BudgetItemId = -2,
                        CategoryId = 1,
                        ScheduledDate = new DateTime(200, 1, 30)
                    },
                    new Transaction()
                    {
                        Name = "expense3",
                        Amount = 10,
                        BudgetItemId = 3,
                        CategoryId = 1,
                        ScheduledDate = new DateTime(1999, 3, 30)
                    },
                    new Transaction()
                    {
                        Name = "expense3",
                        Amount = 10,
                        BudgetItemId = 3,
                        CategoryId = 1,
                        ScheduledDate = new DateTime(2000, 1, 30)
                    },
                    new Transaction()
                    {
                        Name = "expense8",
                        Amount = 88,
                        BudgetItemId = 8,
                        CategoryId = 2,
                        ScheduledDate = new DateTime(2023, 10, 5)
                    },
                    new Transaction()
                    {
                        Name = "expense7",
                        Amount = 777,
                        BudgetItemId = 7,
                        CategoryId = 0,
                        ScheduledDate = new DateTime(2024, 5, 15)
                    },
                    new Transaction()
                    {
                        Name = "expense7",
                        Amount = 777,
                        BudgetItemId = 7,
                        CategoryId = 0,
                        ScheduledDate = new DateTime(2024, 7, 15)
                    },
                    new Transaction()
                    {
                        Name = "income9",
                        Amount = 999,
                        BudgetItemId = 9,
                        CategoryId = 0,
                        ScheduledDate = new DateTime(2024, 11, 15)
                    },
                    new Transaction()
                    {
                        Name = "income10",
                        Amount = 10.10M,
                        BudgetItemId = 10,
                        CategoryId = 1,
                        ScheduledDate = new DateTime(2024, 12, 15)
                    }
                },
                ExpenseIdCounter = 0,
                Wishlist = new Wishlist(),
                CategoryList = new List<Category>() {
                    new Category() { Name = "No Category", Id = 0 },
                    new Category() { Name = "cat1", Id = 1 },
                    new Category() { Name = "cat2", Id = 2 },
                },
                IncomeIdCounter = 0,
                TransactionIdCounter = 0,
                LastLoginDate = new DateTime(2019, 02, 01),
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
                },
                new UserAccount
                {
                    Id = 3,
                    FullName = "Random User",
                    Passcode = "1111",
                    Balance = 0,
                    IsLocked = false,
                    TotalLogin = 0,
                }
            };
            #endregion

        }

        public void LoadDataFromDirectory()
        {
            var fileDataPath = "";
            var userInfoRoot = "";
            var rootFolder = "BudgetAppProject_UserInfo";
            var loadFile = "fileLoadInfo.txt";
            var dataFile = "";
            var directoryList = new List<string>();
            var addMoreDirectoriesToFileDataList = true;

            var loadFromDefaultPathPrompt = Utilities.PromptYesOrNo("Load data from default path?");

            if(loadFromDefaultPathPrompt == "y")
            {
                userInfoRoot = Path.Combine(new DirectoryInfo(".").Parent?.Parent?.Parent?.Parent?.Parent?.FullName ?? "");
                if(!Directory.Exists(Path.Combine(userInfoRoot, "BudgetAppProject_UserInfo")))
                {
                    Console.WriteLine("Default directory folder not found at root.");
                    var defaultNameAtRoot = Utilities.PromptYesOrNo("Use default name for root folder?");
                    if(defaultNameAtRoot == "n")
                    {
                        rootFolder = Utilities.GetUserInput("folder name at root");
                    }
                    userInfoRoot = Directory.CreateDirectory(Path.Combine(userInfoRoot, rootFolder)).ToString();
                }
                else
                {
                    userInfoRoot = Path.Combine(userInfoRoot, "BudgetAppProject_UserInfo");
                }
            }
            else
            {
                userInfoRoot = Utilities.GetUserInput("path to load data");
                if (!Directory.Exists(userInfoRoot))
                {
                    Console.WriteLine("Directory does not exist. Creating new directory");
                    Utilities.PrintDotAnimation();
                    Directory.CreateDirectory(userInfoRoot);
                    while (!Directory.Exists(userInfoRoot))
                    {
                        Utilities.PrintMessage("Invalid directory name. Please try again", false, false);
                        userInfoRoot = Directory.CreateDirectory(Utilities.GetUserInput("path to load data")).ToString();
                    }
                }
            }
            var useDefaultLoadFilePrompt = Utilities.PromptYesOrNo("use default file to load data?");
            if(useDefaultLoadFilePrompt == "y")
            {
                while(!File.Exists(Path.Combine(userInfoRoot, loadFile)))
                {
                    Utilities.PrintMessage("File does not exist or is invalid. Please create a file", false, false);
                    loadFile = Utilities.GetUserInput("file name");
                    File.Create(Path.Combine(userInfoRoot, loadFile));
                }
                fileDataPath = Path.Combine(userInfoRoot, loadFile);
            }
            else
            {
                loadFile = Utilities.GetUserInput("a file name");

                var existingOrNewPrompt = Utilities.PromptYesOrNo("Is this an existing file?");
                if (existingOrNewPrompt == "n")
                {
                    File.Create(Path.Combine(userInfoRoot, loadFile));
                }
                while (!File.Exists(Path.Combine(userInfoRoot, loadFile)))
                {
                    Utilities.PrintMessage("File does not exist or is invalid. Please create a file", false, false);
                    loadFile = Utilities.GetUserInput("file name");
                    File.Create(Path.Combine(userInfoRoot, loadFile));
                }
                
                fileDataPath = Path.Combine(userInfoRoot, loadFile);
            }
            var fileDataContents = new List<string>();

            if (File.ReadAllLines(fileDataPath).Any())
            {
                fileDataContents = File.ReadAllLines(fileDataPath)[0].Split(';').ToList();
            }
            if(!fileDataContents.Any())
            {
                Utilities.PrintMessage("Must have at least 1 directory with data files. Please create one", false, false);
                while (addMoreDirectoriesToFileDataList)
                {
                    var directoryToLoadData = Utilities.GetUserInput("directory name");
                    directoryList.Add(directoryToLoadData);
                    Directory.CreateDirectory(Path.Combine(userInfoRoot, directoryToLoadData));
                    var addAnotherFilePrompt = Utilities.PromptYesOrNo("Add another directory?");
                    if(addAnotherFilePrompt == "n")
                    {
                        addMoreDirectoriesToFileDataList = false;
                    }
                }
            }
            var allDirectoriesAsLine = string.Join(";", directoryList);
            File.WriteAllText(fileDataPath, allDirectoriesAsLine);
            fileDataContents = File.ReadAllLines(fileDataPath)[0].Split(';').ToList();

            Console.WriteLine("Files in file data path. Enter number of file you'd like to load");
            for (var i = 1; i <= fileDataContents.Count(); i++)
            {
                Console.WriteLine($"{i}. {fileDataContents[i - 1]}");
            }
            var fileOption = Validator.Convert<int>("an option");
            while((fileOption < 1) || (fileOption > fileDataContents.Count()))
            {
                Utilities.PrintMessage("Invalid input. Please try again", false, false);
                fileOption = Validator.Convert<int>("an option");
            }

            dataFile = fileDataContents[fileOption - 1];

            var selectedAccount = new UserAccount();
            var userData = Path.Combine(userInfoRoot, dataFile);
            var userInfo = File.ReadAllLines(Path.Combine(userData, "userInfo.txt"))[0].Split(';').ToList(); ;
            var fullName = userInfo[1][(userInfo[1].IndexOf(':') + 1)..];
            var passcode = userInfo[2][(userInfo[2].IndexOf(':') + 1)..];
            selectedAccount.FullName = fullName;
            selectedAccount.Passcode = passcode;
            userAccountList = new List<UserAccount> { selectedAccount };
            Run();
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
                    ProcessBudgetSummaryOption();
                    break;
                case (int)AppMenu.Instructions:
                    DisplayInstructions();
                    Utilities.PressEnterToContinue();
                    break;
                case (int)AppMenu.Incomes:
                    ProcessBudgetItemOption(BudgetItemType.Income);
                    break;
                case (int)AppMenu.Expenses:
                    ProcessBudgetItemOption(BudgetItemType.Expense);
                    break;
                case (int)AppMenu.Categories:
                    ProcessCategoryOption();
                    break;
                case (int)AppMenu.Wishlist:
                    ProcessWishlistOption();
                    break;
                case (int)AppMenu.Logout:
                    LogoutProgress();
                    break;
                case (int)AppMenu.SaveInfo:
                    Utilities.SaveUserInformation(selectedAccount);
                    Utilities.PressEnterToContinue();
                    break;
                case (int)AppMenu.LoadInfo:
                    selectedAccount = Utilities.LoadUserInformation(selectedAccount);
                    Utilities.PressEnterToContinue();
                    break;
                case (int)AppMenu.Other:
                    AppScreen.DisplayInitialTransactionOptions();
                    ProcessGeneralMenuOption();
                    break;
                default:
                    Utilities.PrintMessage("Invalid option.", false);
                    break;
            }
            GoBackToAppScreen();
        }
        #endregion

        
        public void GoBackToAppScreen()
        {
            AppScreen.DisplayAppMenu();
            ProcessAppMenuOption();
        }

        private void ProcessBudgetItemOption(BudgetItemType type)
        {
            AppScreen.DisplayInitialBudgetItemOptions(type);
            ProcessBudgetItemMenuOption(type);
        }

        private void ProcessCategoryOption()
        {
            AppScreen.DisplayInitialCategoryOptions();
            ProcessCategoryMenuOption();
        }

        public void ProcessWishlistOption()
        {
            AppScreen.DisplayInitialWishlistOptions();
            ProcessWishlistMenuOption();
        }

        public void ProcessBudgetSummaryOption()
        {
            AppScreen.DisplayInitialBudgetSummaryOptions();
            ProcessBudgetSummaryMenuOption();
        }

        public void ProcessGeneralMenuOption()
        {
            AppScreen.DisplayGeneralOptions();
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    var budgetItem = FindBudgetItem();
                    if (budgetItem.Id != -1)
                    {
                        budgetItem.DisplayAllTransactionsForItem(selectedAccount);
                    }
                    break;
                case 2:
                    DisplayAllTransactions();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessGeneralMenuOption();
                    break;
            }
        }

        public void PromptUserToSave()
        {
            string prompt = Utilities.PromptYesOrNo("Would you like to save your data?");

            if (prompt == "y")
            {
                Utilities.SaveUserInformation(selectedAccount);
            }
            else if (prompt == "n")
            {
                return;
            }
            else
            {
                throw new Exception();
            }
        }

        public void LogoutProgress()
        {
            Console.WriteLine("Thank you for using My Budget App.");
            Utilities.PrintDotAnimation();
            PromptUserToSave();
            Console.Clear();
            string logoutOption = Utilities.PromptYesOrNo("Would you like to exit the app?").ToLower();

            if (logoutOption == "y")
            {
                Environment.Exit(1);
            }

            Utilities.PrintMessage("You have successfully logged out.", true);
            Run();
        }

        public bool PromptUserToLoadData()
        {
            string prompt = Utilities.PromptYesOrNo("Would you like to load your saved data? ");

            if (prompt == "y")
            {
                selectedAccount = Utilities.LoadUserInformation(selectedAccount);
                return true;
            }
            else if (prompt == "n")
            {
                return false;
            }
            else
            {
                throw new Exception();
            }
        }

        //public void CreatePendingTransactions(int daysInFuture)
        //{
        //    var dateToMatchTransaction = DateTime.Now;

        //    Console.WriteLine("Enter the date range you'd like to create pending transactions");
        //    int startDateMonth = Validator.Convert<int>("month of start date");
        //    int startDateDay = Validator.Convert<int>("day of start date");
        //    int startDateYear = Validator.Convert<int>("year of start date");
        //    int endDateMonth = Validator.Convert<int>("month of end date");
        //    int endDateDay = Validator.Convert<int>("day of end date");
        //    int endDateYear = Validator.Convert<int>("year of end date");

        //    var startDate = new DateTime(startDateYear, startDateMonth, startDateDay);
        //    var endDate = new DateTime(endDateYear, endDateMonth, endDateDay);

        //    foreach (Transaction transaction in selectedAccount.TransactionList)
        //    {
        //        var listOfDatesToMatch = FindDatesOfTransactionsInTimeRange(transaction, startDate, endDate);

        //        if (TransationList.Find)
        //    }

        //    foreach (Transaction transaction in selectedAccount.IncomeList)
        //    {

        //    }
        //}

        //public List<DateTime> FindDatesOfTransactionsInTimeRange(Transaction transaction, DateTime startDate, DateTime endDate)
        //{
        //    int daysBetweenIntervals = 0;
        //    var datesList = new List<DateTime>();
        //    var roughDate = new DateTime(Math.Max(startDate.Ticks, transaction.Date.Ticks));
        //    var exactDate = transaction.Date;

        //    switch (transaction.Rate)
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
        //        case Rate.Other:
        //        case Rate.NoRate:
        //            Utilities.PrintMessage("Invalid transaction due to inconsistent or nonexistent rate.", false, false);
        //            break;
        //    }

        //    if(roughDate != transaction.Date)
        //    {
        //        while(exactDate < roughDate)
        //        {
        //            exactDate = exactDate.AddDays(daysBetweenIntervals);
        //        }
        //    }
        //    else
        //    {
        //        exactDate = roughDate.Date;
        //    }

        //    while(exactDate < endDate)
        //    {
        //        datesList.Add(exactDate);
        //        exactDate = exactDate.AddDays(daysBetweenIntervals);
        //    }

        //    return datesList;
        //}

        //public void CreateTransactionsOverTime()
        //{

        //}

        //public decimal CalculateIncomeByRateAndTime(TimeRange timeRange, Income income)
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

        //        startTimeSpan = DateTime.Now;
        //            endTimeSpan = endTime;

        //            while (currentPayPeriod<startTimeSpan)
        //            {
        //                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //            }

        //            while (currentPayPeriod<endTimeSpan)
        //            {
        //                payPeriodCounter++;
        //                currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //            }

        //pay *= payPeriodCounter;
        //return pay;
        //        }

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

        //public int CalculateTransactionCounter(DateTime endTimeSpan, Rate rate)
        //{
        //    DateTime firstPayPeriod = new(DateTime.Now.Year, 1, 5);
        //    DateTime currentPayPeriod = firstPayPeriod;
        //    int payPeriodCounter = 0;
        //    int daysBetweenIntervals;

        //    switch (rate)
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
        //        default:
        //            daysBetweenIntervals = 14;
        //            break;
        //    }

        //    while (currentPayPeriod < DateTime.Now)
        //    {
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    while (currentPayPeriod < endTimeSpan)
        //    {
        //        payPeriodCounter++;
        //        currentPayPeriod = currentPayPeriod.AddDays(daysBetweenIntervals);
        //    }

        //    return payPeriodCounter;
        //}

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

        public Rate ProcessRateOption()
        {
            string isRepeating = Utilities.PromptYesOrNo("Is this a repeating transaction?");

            if (isRepeating == "y")
            {
                AppScreen.DisplayRateOptions();

                int rateInt = Validator.Convert<int>("a rate");
                var checkRate = Utilities.RateIsInRange(rateInt);

                while (!checkRate)
                {
                    Utilities.PrintMessage("Rate out of range. Please try again", false, true);
                    rateInt = Validator.Convert<int>("a rate");
                    checkRate = Utilities.RateIsInRange(rateInt);
                }

                var rate = (Rate)rateInt;

                return rate;
            }
            else if(isRepeating == "n")
            {
                return Rate.NoRate;
            }
            else
            {
                throw new Exception();
            }
        }

        #region Methods That Need Organizing/Implementing

        protected bool PreviewUpdate(decimal amount)
        {
            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{Utilities.FormatAmount(amount)}\n\n");
            Console.WriteLine("");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }
        #endregion
    }

    // TODO: Add logic to do time-span-based calculations with budget summary, expenses, incomes, categories, and wishlist
}

