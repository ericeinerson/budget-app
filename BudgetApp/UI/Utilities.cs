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

                string fullName = userAccountInfoSplits[0][(userAccountInfoSplits[0].IndexOf(':') + 1)..];
                string passcode = userAccountInfoSplits[1][(userAccountInfoSplits[1].IndexOf(':') + 1)..];
                int id = int.Parse(userAccountInfoSplits[2][(userAccountInfoSplits[2].IndexOf(':') + 1)..]);
                bool isLocked = bool.Parse(userAccountInfoSplits[3].AsSpan(userAccountInfoSplits[3].IndexOf(':') + 1));
                int totalLogin = int.Parse(userAccountInfoSplits[4][(userAccountInfoSplits[4].IndexOf(':') + 1)..]);
                decimal balance = decimal.Parse(userAccountInfoSplits[5][(userAccountInfoSplits[5].IndexOf(':') + 1)..]);
                string directory = userAccountInfoSplits[6][(userAccountInfoSplits[6].IndexOf(':') + 1)..];
                int incomeId = int.Parse(userAccountInfoSplits[7][(userAccountInfoSplits[7].IndexOf(':') + 1)..]);
                int expenseId = int.Parse(userAccountInfoSplits[8][(userAccountInfoSplits[8].IndexOf(':') + 1)..]);
                int wishlistId = int.Parse(userAccountInfoSplits[9][(userAccountInfoSplits[9].IndexOf(':') + 1)..]);
                DateTime lastLoginDate = DateTime.Parse(userAccountInfoSplits[10][(userAccountInfoSplits[10].IndexOf(':') + 1)..]);


                userAccount.FullName = fullName;
                userAccount.Passcode = passcode;
                userAccount.Id = id;
                userAccount.IsLocked = isLocked;
                userAccount.TotalLogin = totalLogin;
                userAccount.Balance = balance;
                userAccount.Directory = directory;
                userAccount.IncomeId = incomeId;
                userAccount.ExpenseId = expenseId;
                userAccount.WishlistId = wishlistId;
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

                    string expenseName = expensesSplits[0].Substring(expensesSplits[0].IndexOf(':') + 1);
                    decimal amount = decimal.Parse(expensesSplits[1].Substring(expensesSplits[1].IndexOf(':') + 1));
                    DateTime date = DateTime.Parse(expensesSplits[2].Substring(expensesSplits[2].IndexOf(':') + 1));
                    Rate rate = (Rate)int.Parse(expensesSplits[3].Substring(expensesSplits[3].IndexOf(':') + 1));
                    string amountFormatted = expensesSplits[4].Substring(expensesSplits[4].IndexOf(':') + 1);
                    int id = int.Parse(expensesSplits[5].Substring(expensesSplits[5].IndexOf(':') + 1));
                    int categoryId = int.Parse(expensesSplits[6].Substring(expensesSplits[6].IndexOf(':') + 1));
                    TransactionType transactionType = (TransactionType)int.Parse(expensesSplits[7].Substring(expensesSplits[7].IndexOf(':') + 1));
                    Status status = (Status)int.Parse(expensesSplits[8].Substring(expensesSplits[8].IndexOf(':') + 1));

                    userAccount.ExpenseList.Add(new Transaction()
                    {
                        Name = expenseName,
                        Amount = amount,
                        Date = date,
                        Rate = rate,
                        AmountFormatted = amountFormatted,
                        Id = id,
                        CategoryId = categoryId,
                        TransactionType = transactionType,
                        Status = status
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

                    string incomeName = incomesSplits[0].Substring(incomesSplits[0].IndexOf(':') + 1);
                    decimal amount = decimal.Parse(incomesSplits[1].Substring(incomesSplits[1].IndexOf(':') + 1));
                    DateTime date = DateTime.Parse(incomesSplits[2].Substring(incomesSplits[2].IndexOf(':') + 1));
                    Rate rate = (Rate)int.Parse(incomesSplits[3].Substring(incomesSplits[3].IndexOf(':') + 1));
                    string amountFormatted = incomesSplits[4].Substring(incomesSplits[4].IndexOf(':') + 1);
                    int id = int.Parse(incomesSplits[5].Substring(incomesSplits[5].IndexOf(':') + 1));
                    int categoryId = int.Parse(incomesSplits[6].Substring(incomesSplits[6].IndexOf(':') + 1));
                    TransactionType transactionType = (TransactionType)int.Parse(incomesSplits[7].Substring(incomesSplits[7].IndexOf(':') + 1));
                    Status status = (Status)int.Parse(incomesSplits[8].Substring(incomesSplits[8].IndexOf(':') + 1));


                    userAccount.IncomeList.Add(new Transaction()
                    {
                        Name = incomeName,
                        Amount = amount,
                        Date = date,
                        Rate = rate,
                        AmountFormatted = amountFormatted,
                        Id = id,
                        CategoryId = categoryId,
                        TransactionType = transactionType,
                        Status = status
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
            if (File.Exists(categoriesPath))
            {
                string[] categoriesAsString = File.ReadAllLines(categoriesPath);

                for (int i = 0; i < categoriesAsString.Length; i++)
                {
                    List<string>? categoriesSplits = categoriesAsString[i].Split(';').ToList();

                    string categoryName = categoriesSplits[0].Substring(categoriesSplits[0].IndexOf(':') + 1);
                    int id = int.Parse(categoriesSplits[1].Substring(categoriesSplits[1].IndexOf(':') + 1));
                    
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
            string categoriesPath = @$"{userAccount.Directory}{categoriesInfoFileName}";
            StringBuilder categoriesSB = new();
            string wishlistPath = @$"{userAccount.Directory}{wishlistInfoFileName}";
            StringBuilder wishlistSB = new();

            userInfoSB.Append($"fullName:{userAccount.FullName};");
            userInfoSB.Append($"passcode:{userAccount.Passcode.ToString()};");
            userInfoSB.Append($"id:{userAccount.Id};");
            userInfoSB.Append($"isLocked:{userAccount.IsLocked};");
            userInfoSB.Append($"totalLogin:{userAccount.TotalLogin};");
            userInfoSB.Append($"balance:{userAccount.Balance};");
            userInfoSB.Append($"directory:{userAccount.Directory};");
            userInfoSB.Append($"income id:{userAccount.IncomeId};");
            userInfoSB.Append($"expense id:{userAccount.ExpenseId};");
            userInfoSB.Append($"wishlist id:{userAccount.WishlistId};");
            userInfoSB.Append($"last login date:{userAccount.LastLoginDate};");

            userInfoSB.Append(Environment.NewLine);

            foreach (Transaction e in userAccount.ExpenseList)
            {
                expensesSB.Append($"expenseName:{e.Name};");
                expensesSB.Append($"amount:{e.Amount};");
                expensesSB.Append($"date:{e.Date};");
                expensesSB.Append($"rate:{(int)e.Rate};");
                expensesSB.Append($"amount formatted:{e.AmountFormatted};");
                expensesSB.Append($"id:{e.Id};");
                expensesSB.Append($"category id:{e.CategoryId};");
                expensesSB.Append($"transaction type:{(int)e.TransactionType};");
                expensesSB.Append($"status:{(int)e.Status};");
                expensesSB.Append(Environment.NewLine);
            }
            foreach (Transaction i in userAccount.IncomeList)
            {
                incomesSB.Append($"incomeName:{i.Name};");
                incomesSB.Append($"amount:{i.Amount};");
                incomesSB.Append($"date:{i.Date};");
                incomesSB.Append($"rate:{(int)i.Rate};");
                incomesSB.Append($"amount formatted:{i.AmountFormatted};");
                incomesSB.Append($"id:{i.Id};");
                incomesSB.Append($"category id:{i.CategoryId};");
                incomesSB.Append($"transaction type:{(int)i.TransactionType};");
                incomesSB.Append($"status:{(int)i.Status};");
                incomesSB.Append(Environment.NewLine);
            }
            foreach (Category c in userAccount.CategoryList)
            {
                categoriesSB.Append($"categoryName:{c.Name};");
                categoriesSB.Append($"amount:{c.Id};");
                categoriesSB.Append(Environment.NewLine);
            }
            foreach (WishlistItem w in userAccount.Wishlist.Items)
            {
                wishlistSB.Append($"itemName:{w.Item};");
                wishlistSB.Append($"cost:{w.Cost};");
                wishlistSB.Append($"id:{w.Id};");
                wishlistSB.Append($"priority:{w.Priority};");
                wishlistSB.Append(Environment.NewLine);
            }

            File.WriteAllText(userInfoPath, userInfoSB.ToString());
            File.WriteAllText(expensesPath, expensesSB.ToString());
            File.WriteAllText(incomesPath, incomesSB.ToString());
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

            userInfoSB.Append($"fullName:{userAccount.FullName};");
            userInfoSB.Append($"passcode:{userAccount.Passcode.ToString()};");
            userInfoSB.Append($"id:{userAccount.Id};");
            userInfoSB.Append($"isLocked:{userAccount.IsLocked};");
            userInfoSB.Append($"totalLogin:{userAccount.TotalLogin};");
            userInfoSB.Append($"balance:{userAccount.Balance};");
            userInfoSB.Append($"directory:{userAccount.Directory};");
            userInfoSB.Append($"income id:{userAccount.IncomeId};");
            userInfoSB.Append($"expense id:{userAccount.ExpenseId};");
            userInfoSB.Append($"wishlist id:{userAccount.WishlistId};");
            userInfoSB.Append($"last login date:{userAccount.LastLoginDate};");
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

