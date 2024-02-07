using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;

namespace BudgetApp.UI
{
	public static class Utilities
	{
        private static readonly string basicUserInfoFileName = $"userInfo.txt";
        private static readonly string expensesInfoFileName = "userExpenses.txt";
        private static readonly string incomesInfoFileName = "userIncomes.txt";
        private static readonly string transactionsInfoFileName = "userTransactions.txt";
        private static readonly string categoriesInfoFileName = "userCategories.txt";
        private static readonly string wishlistInfoFileName = "userWishlist.txt";

        private static readonly CultureInfo culture = new("EN-US");
        private static long transactionId;

        public static long GetTransactionId()
        {
            return ++transactionId;
        }

        public static string GetSecretInput(string prompt)
        {
            bool isPrompt = true;
            string asterics = string.Empty;

            StringBuilder input = new();

            while (true)
            {
                if (isPrompt)
                {
                    Console.WriteLine(prompt);
                }
                isPrompt = false;

                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                if(inputKey.Key == ConsoleKey.Enter)
                {
                    if(input.Length == 4)
                    {
                        break;
                    }
                    else
                    {
                        PrintMessage("\nPlease enter 4 digits.", false);
                        input.Clear();
                        isPrompt = true;
                        continue;
                    }
                }
                if(inputKey.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length -1, 1);
                    Console.Write("\b \b");
                }
                else if(inputKey.Key != ConsoleKey.Backspace)
                {
                    input.Append(inputKey.KeyChar);
                    Console.Write(asterics + "*");
                }
            }
            return input.ToString();
        }

        public static void PrintMessage(string msg, bool success, bool next = false)
        {
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (next == false)
            {
                PressEnterToContinue();
            }
        }

        public static string GetUserInput(string prompt)
        {
            Console.WriteLine($"Enter {prompt}");
            
            return Console.ReadLine() ?? string.Empty;
        }

        public static void PrintDotAnimation(int timer = 10)
        {
            for (int i = 0; i < timer; i++)
            {
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.Clear();
        }

        public static void PressEnterToContinue()
        {
            Console.WriteLine("\n\nPress enter to continue...\n");
            Console.ReadLine();
        }

        public static string FormatAmount(decimal amt)
        {
            return string.Format(culture, "{0:C2}", amt);
        }

        public static UserAccount LoadUserInformation(UserAccount userAccount)
        {
            bool isLoaded = false;
            string userAccountInfoPath = @$"{userAccount.Directory}{basicUserInfoFileName}";
            string expensesPath = @$"{userAccount.Directory}{expensesInfoFileName}";
            string incomesPath = @$"{userAccount.Directory}{incomesInfoFileName}";
            string transactionsPath = @$"{userAccount.Directory}{transactionsInfoFileName}";
            string categoriesPath = @$"{userAccount.Directory}{categoriesInfoFileName}";
            string wishlistPath = @$"{userAccount.Directory}{wishlistInfoFileName}";

            userAccount = new UserAccount();

            if (File.Exists(userAccountInfoPath))
            {
                string[] userAccountInfoAsString = File.ReadAllLines(userAccountInfoPath);
                var userAccountInfoSplits = new List<string>();

                for (int i = 0; i < userAccountInfoAsString.Length; i++)
                {
                    userAccountInfoSplits = userAccountInfoAsString[i].Split(';').ToList();
                }
                int id = int.Parse(userAccountInfoSplits[0][(userAccountInfoSplits[0].IndexOf(':') + 1)..]);
                string fullName = userAccountInfoSplits[1][(userAccountInfoSplits[1].IndexOf(':') + 1)..];
                string passcode = userAccountInfoSplits[2][(userAccountInfoSplits[2].IndexOf(':') + 1)..];
                bool isLocked = bool.Parse(userAccountInfoSplits[3].AsSpan(userAccountInfoSplits[3].IndexOf(':') + 1));
                int totalLogin = int.Parse(userAccountInfoSplits[4][(userAccountInfoSplits[4].IndexOf(':') + 1)..]);
                decimal balance = decimal.Parse(userAccountInfoSplits[5][(userAccountInfoSplits[5].IndexOf(':') + 1)..]);
                string directory = userAccountInfoSplits[6][(userAccountInfoSplits[6].IndexOf(':') + 1)..];
                int incomeId = int.Parse(userAccountInfoSplits[7][(userAccountInfoSplits[7].IndexOf(':') + 1)..]);
                int expenseId = int.Parse(userAccountInfoSplits[8][(userAccountInfoSplits[8].IndexOf(':') + 1)..]);
                int wishlistId = int.Parse(userAccountInfoSplits[9][(userAccountInfoSplits[9].IndexOf(':') + 1)..]);
                int budgetItemId = int.Parse(userAccountInfoSplits[10][(userAccountInfoSplits[10].IndexOf(':') + 1)..]);
                int transactionId = int.Parse(userAccountInfoSplits[11][(userAccountInfoSplits[11].IndexOf(':') + 1)..]);
                DateTime lastLoginDate = DateTime.Parse(userAccountInfoSplits[12][(userAccountInfoSplits[12].IndexOf(':') + 1)..]);

                userAccount.Id = id;
                userAccount.FullName = fullName;
                userAccount.Passcode = passcode;
                userAccount.IsLocked = isLocked;
                userAccount.TotalLogin = totalLogin;
                userAccount.Balance = balance;
                userAccount.Directory = directory;
                userAccount.IncomeIdCounter = incomeId;
                userAccount.ExpenseIdCounter = expenseId;
                userAccount.WishlistIdCounter = wishlistId;
                userAccount.BudgetItemIdCounter = budgetItemId;
                userAccount.TransactionIdCounter = transactionId;
                userAccount.LastLoginDate = lastLoginDate;

                if (userAccountInfoAsString.Length > 0)
                {
                    PrintMessage("User account loaded!", true, true);
                    isLoaded = true;
                }
                else
                {
                    PrintMessage("No user account info found", false, true);
                }
            }
            if (File.Exists(expensesPath))
            {
                string[] expensesAsString = File.ReadAllLines(expensesPath);

                for (int i = 0; i < expensesAsString.Length; i++)
                {
                    List<string>? expensesSplits = expensesAsString[i].Split(';').ToList();

                    int id = int.Parse(expensesSplits[0].Substring(expensesSplits[0].IndexOf(':') + 1));
                    int expenseId = int.Parse(expensesSplits[1].Substring(expensesSplits[1].IndexOf(':') + 1));
                    int categoryId = int.Parse(expensesSplits[2].Substring(expensesSplits[2].IndexOf(':') + 1));
                    string expenseName = expensesSplits[3].Substring(expensesSplits[3].IndexOf(':') + 1);
                    decimal amount = decimal.Parse(expensesSplits[4].Substring(expensesSplits[4].IndexOf(':') + 1));
                    DateTime startDate = DateTime.Parse(expensesSplits[5].Substring(expensesSplits[5].IndexOf(':') + 1));
                    DateTime endDate = DateTime.Parse(expensesSplits[6].Substring(expensesSplits[6].IndexOf(':') + 1));
                    Rate rate = (Rate)int.Parse(expensesSplits[7].Substring(expensesSplits[7].IndexOf(':') + 1));

                    userAccount.ExpenseList.Add(new Expense()
                    {
                        Id = id,
                        ExpenseId = expenseId,
                        CategoryId = categoryId,
                        Name = expenseName,
                        Amount = amount,
                        StartDate = startDate,
                        EndDate = endDate,
                        Rate = rate,
                    });
                }
                if (expensesAsString.Length > 0)
                {
                    PrintMessage("Expenses loaded!", true, true);
                    isLoaded = true;
                }
                else
                {
                    PrintMessage("No expenses found", false, true);
                }
            }
            if (File.Exists(incomesPath))
            {
                string[] incomesAsString = File.ReadAllLines(incomesPath);

                for (int i = 0; i < incomesAsString.Length; i++)
                {
                    List<string>? incomesSplits = incomesAsString[i].Split(';').ToList();

                    int id = int.Parse(incomesSplits[0].Substring(incomesSplits[0].IndexOf(':') + 1));
                    int incomeId = int.Parse(incomesSplits[1].Substring(incomesSplits[1].IndexOf(':') + 1));
                    int categoryId = int.Parse(incomesSplits[2].Substring(incomesSplits[2].IndexOf(':') + 1));
                    string incomeName = incomesSplits[3].Substring(incomesSplits[3].IndexOf(':') + 1);
                    decimal amount = decimal.Parse(incomesSplits[4].Substring(incomesSplits[4].IndexOf(':') + 1));
                    DateTime startDate = DateTime.Parse(incomesSplits[5].Substring(incomesSplits[5].IndexOf(':') + 1));
                    DateTime endDate = DateTime.Parse(incomesSplits[6].Substring(incomesSplits[6].IndexOf(':') + 1));
                    Rate rate = (Rate)int.Parse(incomesSplits[7].Substring(incomesSplits[7].IndexOf(':') + 1));

                    userAccount.IncomeList.Add(new Income()
                    {
                        Id = id,
                        IncomeId = incomeId,
                        CategoryId = categoryId,
                        Name = incomeName,
                        Amount = amount,
                        StartDate = startDate,
                        EndDate = endDate,
                        Rate = rate,
                    });
                }
                if (incomesAsString.Length > 0)
                {
                    PrintMessage("Incomes loaded!", true, true);
                    isLoaded = true;
                }
                else
                {
                    PrintMessage("No incomes found", false, true);
                }
            }
            if (File.Exists(transactionsPath))
            {
                string[] transactionsAsString = File.ReadAllLines(transactionsPath);

                for (int i = 0; i < transactionsAsString.Length; i++)
                {
                    List<string>? transactionsSplits = transactionsAsString[i].Split(';').ToList();

                    int id = int.Parse(transactionsSplits[0][(transactionsSplits[0].IndexOf(':') + 1)..]);
                    int categoryId = int.Parse(transactionsSplits[1][(transactionsSplits[1].IndexOf(':') + 1)..]);
                    int budgetItemId = int.Parse(transactionsSplits[2][(transactionsSplits[2].IndexOf(':') + 1)..]);
                    string transactionName = transactionsSplits[3][(transactionsSplits[3].IndexOf(':') + 1)..];
                    decimal amount = decimal.Parse(transactionsSplits[4][(transactionsSplits[4].IndexOf(':') + 1)..]);
                    DateTime createdDate = DateTime.Parse(transactionsSplits[5][(transactionsSplits[5].IndexOf(':') + 1)..]);
                    DateTime postedDate = DateTime.Parse(transactionsSplits[6][(transactionsSplits[6].IndexOf(':') + 1)..]);
                    BudgetItemType type = (BudgetItemType)int.Parse(transactionsSplits[7].Substring(transactionsSplits[7].IndexOf(':') + 1));
                    Status status = (Status)int.Parse(transactionsSplits[8][(transactionsSplits[8].IndexOf(':') + 1)..]);

                    userAccount.TransactionList.Add(new Transaction()
                    {
                        Id = id,
                        CategoryId = categoryId,
                        BudgetItemId = budgetItemId,
                        Name = transactionName,
                        Amount = amount,
                        CreatedDate = createdDate,
                        PostedDate = postedDate,
                        BudgetItemType = type,
                        Status = status
                    });
                }
                if (transactionsAsString.Length > 0)
                {
                    PrintMessage("Transactions loaded!", true, true);
                    isLoaded = true;
                }
                else
                {
                    PrintMessage("No transactions found", false, true);
                }
            }
            if (File.Exists(categoriesPath))
            {
                string[] categoriesAsString = File.ReadAllLines(categoriesPath);

                for (int i = 0; i < categoriesAsString.Length; i++)
                {
                    List<string>? categoriesSplits = categoriesAsString[i].Split(';').ToList();

                    int id = int.Parse(categoriesSplits[0].Substring(categoriesSplits[0].IndexOf(':') + 1));
                    string categoryName = categoriesSplits[1].Substring(categoriesSplits[1].IndexOf(':') + 1);
                    
                    userAccount.CategoryList.Add(new Category()
                    {
                        Name = categoryName,
                        Id = id
                    });
                }
                if (categoriesAsString.Length > 0)
                {
                    PrintMessage("Categories loaded!", true, true);
                    isLoaded = true;
                }
                else
                {
                    PrintMessage("No categories found", false, true);
                }
            }
            if (File.Exists(wishlistPath))
            {
                string[] wishlistAsString = File.ReadAllLines(wishlistPath);

                for (int i = 0; i < wishlistAsString.Length; i++)
                {
                    string[] wishlistSplits = wishlistAsString[i].Split(';');

                    string item = wishlistSplits[0].Substring(wishlistSplits[0].IndexOf(':') + 1);
                    decimal cost = decimal.Parse(wishlistSplits[1].Substring(wishlistSplits[1].IndexOf(':') + 1));
                    int id = int.Parse(wishlistSplits[2].Substring(wishlistSplits[2].IndexOf(':') + 1));
                    int priority = int.Parse(wishlistSplits[3].Substring(wishlistSplits[3].IndexOf(':') + 1));

                    userAccount.Wishlist.Items.Add(new WishlistItem()
                    {
                        Item = item,
                        Cost = cost,
                        Id = id,
                        Priority = priority
                    });
                }
                if(wishlistAsString.Length > 0)
                {
                    PrintMessage("Wishlist loaded!", true, true);
                    isLoaded = true;
                }
                else
                {
                    PrintMessage("No wishlist found", false, true);
                }
            }

            if (isLoaded)
            {
                PrintMessage("User data successfully loaded", true, true);
            }
            else
            {
                PrintMessage("User data not found", false, true);
            }

            return userAccount;
        }

        public static void SaveUserInformation(UserAccount userAccount)
        {
            string userInfoPath = @$"{userAccount.Directory}{basicUserInfoFileName}";
            StringBuilder userInfoSB = new();
            string expensesPath = @$"{userAccount.Directory}{expensesInfoFileName}";
            StringBuilder expensesSB = new();
            string incomesPath = @$"{userAccount.Directory}{incomesInfoFileName}";
            StringBuilder incomesSB = new();
            string transactionsPath = $@"{userAccount.Directory}{transactionsInfoFileName}";
            StringBuilder transacctionsSB = new();
            string categoriesPath = @$"{userAccount.Directory}{categoriesInfoFileName}";
            StringBuilder categoriesSB = new();
            string wishlistPath = @$"{userAccount.Directory}{wishlistInfoFileName}";
            StringBuilder wishlistSB = new();

            userInfoSB.Append($"id:{userAccount.Id};");//0
            userInfoSB.Append($"fullName:{userAccount.FullName};");//1
            userInfoSB.Append($"passcode:{userAccount.Passcode.ToString()};");//2
            userInfoSB.Append($"isLocked:{userAccount.IsLocked};");//3
            userInfoSB.Append($"totalLogin:{userAccount.TotalLogin};");//4
            userInfoSB.Append($"balance:{userAccount.Balance};");//5
            userInfoSB.Append($"directory:{userAccount.Directory};");//6
            userInfoSB.Append($"income id:{userAccount.IncomeIdCounter};");//7
            userInfoSB.Append($"expense id:{userAccount.ExpenseIdCounter};");//8
            userInfoSB.Append($"wishlist id:{userAccount.WishlistIdCounter};");//9
            userInfoSB.Append($"budget item id:{userAccount.BudgetItemIdCounter};");//10
            userInfoSB.Append($"transaction id:{userAccount.TransactionIdCounter};");//11
            userInfoSB.Append($"last login date:{userAccount.LastLoginDate};");//12

            userInfoSB.Append(Environment.NewLine);

            foreach (Expense b in userAccount.ExpenseList)
            {
                expensesSB.Append($"id:{b.Id};");//0
                expensesSB.Append($"expense id:{b.ExpenseId};");//1
                expensesSB.Append($"category id:{b.CategoryId};");//2
                expensesSB.Append($"expenseName:{b.Name};");//3
                expensesSB.Append($"amount:{b.Amount};");//4
                expensesSB.Append($"start date:{b.StartDate};");//5
                expensesSB.Append($"end date:{b.EndDate};");//6
                expensesSB.Append($"rate:{(int)b.Rate};");//7
                expensesSB.Append(Environment.NewLine);
            }
            foreach (Income b in userAccount.IncomeList)
            {
                incomesSB.Append($"id:{b.Id};");//0
                incomesSB.Append($"income id:{b.IncomeId};");//1
                incomesSB.Append($"category id:{b.CategoryId};");//2
                incomesSB.Append($"incomeName:{b.Name};");//3
                incomesSB.Append($"amount:{b.Amount};");//4
                incomesSB.Append($"start date:{b.StartDate};");//5
                incomesSB.Append($"end date:{b.EndDate};");//6
                incomesSB.Append($"rate:{(int)b.Rate};");//7
                incomesSB.Append(Environment.NewLine);
            }
            foreach (Transaction t in userAccount.TransactionList)
            {
                incomesSB.Append($"id:{t.Id};"); //0
                incomesSB.Append($"category id:{t.CategoryId};"); //1
                incomesSB.Append($"budget item id:{t.BudgetItemId};"); //2
                incomesSB.Append($"incomeName:{t.Name};");//3
                incomesSB.Append($"amount:{t.Amount};");//4
                incomesSB.Append($"created date:{t.CreatedDate};");//5
                incomesSB.Append($"posted date:{t.PostedDate}");//6
                incomesSB.Append($"budget item type:{(int)t.BudgetItemType}");//7
                incomesSB.Append($"status:{(int)t.Status}");//8
                incomesSB.Append(Environment.NewLine);
            }
            foreach (Category c in userAccount.CategoryList)
            {
                categoriesSB.Append($"amount:{c.Id};");//0
                categoriesSB.Append($"categoryName:{c.Name};");//1
                categoriesSB.Append(Environment.NewLine);
            }
            foreach (WishlistItem w in userAccount.Wishlist.Items)
            {
                wishlistSB.Append($"id:{w.Id};");//0
                wishlistSB.Append($"itemName:{w.Item};");//1
                wishlistSB.Append($"cost:{w.Cost};");//2
                wishlistSB.Append($"priority:{w.Priority};");//3
                wishlistSB.Append(Environment.NewLine);
            }

            File.WriteAllText(userInfoPath, userInfoSB.ToString());
            File.WriteAllText(expensesPath, expensesSB.ToString());
            File.WriteAllText(incomesPath, incomesSB.ToString());
            File.WriteAllText(transactionsPath, transacctionsSB.ToString());
            File.WriteAllText(categoriesPath, categoriesSB.ToString());
            File.WriteAllText(wishlistPath, wishlistSB.ToString());
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Saved user info successfully!");
        }

        public static void SaveUserInfoOnlyWithNewLoginTime(UserAccount userAccount)
        {
            DateTime loginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            userAccount.LastLoginDate = loginDate;

            string userInfoPath = @$"{userAccount.Directory}{basicUserInfoFileName}";
            StringBuilder userInfoSB = new();

            userInfoSB.Append($"id:{userAccount.Id};");//0
            userInfoSB.Append($"fullName:{userAccount.FullName};");//1
            userInfoSB.Append($"passcode:{userAccount.Passcode.ToString()};");//2
            userInfoSB.Append($"isLocked:{userAccount.IsLocked};");//3
            userInfoSB.Append($"totalLogin:{userAccount.TotalLogin};");//4
            userInfoSB.Append($"balance:{userAccount.Balance};");//5
            userInfoSB.Append($"directory:{userAccount.Directory};");//6
            userInfoSB.Append($"income id:{userAccount.IncomeIdCounter};");//7
            userInfoSB.Append($"expense id:{userAccount.ExpenseIdCounter};");//8
            userInfoSB.Append($"wishlist id:{userAccount.WishlistIdCounter};");//9
            userInfoSB.Append($"budget item id:{userAccount.BudgetItemIdCounter};");//10
            userInfoSB.Append($"transaction id:{userAccount.TransactionIdCounter};");//11
            userInfoSB.Append($"last login date:{userAccount.LastLoginDate};");//12
            userInfoSB.Append(Environment.NewLine);

            File.WriteAllText(userInfoPath, userInfoSB.ToString());

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Saved user account info successfully with new login time of {userAccount.LastLoginDate}!");
            PressEnterToContinue();
        }

        public static bool CheckForExistingUserFile(UserAccount userAccount)
        {
            bool isSavedData = false;
            string basicUserInfoPath = $"{userAccount.Directory}{basicUserInfoFileName}.txt";
            bool existingBasicUserInfoFileFound = File.Exists(basicUserInfoPath);
            string expensesPath = $"{userAccount.Directory}{expensesInfoFileName}";
            bool existingExpensesFileFound = File.Exists(expensesPath);
            string incomesPath = $"{userAccount.Directory}{incomesInfoFileName}";
            bool existingIncomesFileFound = File.Exists(incomesPath);
            string transactionsPath = $"{userAccount.Directory}{transactionsInfoFileName}";
            bool existingTransactionnsFileFount = File.Exists(transactionsPath);
            string wishlistPath = $"{userAccount.Directory}{wishlistInfoFileName}";
            bool existingWishlistFileFound = File.Exists(wishlistPath);
            string categoryPath = $"{userAccount.Directory}{categoriesInfoFileName}";
            bool existingCategoryFileFound = File.Exists(categoryPath);

            if (existingBasicUserInfoFileFound)
            {
                Console.WriteLine("Existing file for basic user info found");
                isSavedData = true;
            }
            if (existingExpensesFileFound)
            {
                Console.WriteLine("Existing file for expenses found");
                isSavedData = true;
            }
            if (existingIncomesFileFound)
            {
                Console.WriteLine("Existing file for incomes found");
                isSavedData = true;
            }
            if (existingTransactionnsFileFount)
            {
                Console.WriteLine("Existing file for transactions found");
                isSavedData = true;
            }
            if (existingWishlistFileFound)
            {
                Console.WriteLine("Existing file for wishlist found");
                isSavedData = true;
            }

            if (existingCategoryFileFound)
            {
                Console.WriteLine("Existing file for categories found");
                isSavedData = true;

            }
            if (!Directory.Exists(userAccount.Directory))
            {
                string prompt = PromptYesOrNo("No directory found. Would you like to create a directory?");
                if (prompt == "y")
                {
                    Directory.CreateDirectory(userAccount.Directory);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Directory is ready for saving expenses info files");
                    Console.ResetColor();
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

            return isSavedData;
        }



        public static string PromptYesOrNo(string prompt)
        {
            string response = string.Empty;

            while (response != "y" || response != "n")
            {
                Console.WriteLine(prompt + " Please enter Y for yes or N for no.");
                response = Console.ReadLine() ?? string.Empty;
                if (response != null)
                {
                    response = response.ToLower();
                }

                if (response == "y" || response == "n")
                {
                    break;
                }

                PrintMessage("Invalid entry. Try again", false, true);
                continue;
            }
            return response;
        }

        //static bool IsLeapYear(int year)
        //{
        //    if ((year % 400 == 0) ||
        //       (year % 100 != 0) &&
        //       (year % 4 == 0))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public static DateTime ConstructDate()
        {
            int month = Validator.Convert<int>("month transaction is done");
            var longerMonths = new int[7] { 1, 3, 5, 7, 8, 10, 12 };
            var shorterMonths = new int[4] { 4, 6, 9, 11 };

            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("month");
            }
            int day = Validator.Convert<int>("day transaction is done");

            if (day < 1)
            {
                throw new ArgumentOutOfRangeException("day");
            }
            else if (longerMonths.Contains(month) && day > 31)
            {
                throw new ArgumentOutOfRangeException("day");
            }
            else if (shorterMonths.Contains(month) && day > 30)
            {
                throw new ArgumentOutOfRangeException("day");
            }
            else if (month == 2 && day > 29)
            {
                throw new ArgumentOutOfRangeException("day");
            }
            int year = Validator.Convert<int>("year transaction is done");
            if (!DateTime.IsLeapYear(year) && month == 2 && day == 29)
            {
                throw new ArgumentOutOfRangeException("day");
            }
            var date = new DateTime(year, month, day);

            return date;
        }

        //public bool CreatedSinceLastLogin(DateTime lastLoginDate, DateTime transactionDate)
        //{

        //}
    }
}

