using System;
using System.Globalization;
using System.Text;
using BudgetApp.Domain.Entities;
using System.Collections.Generic;
namespace BudgetApp.UI
{
	public static class Utilities
	{
        private static string basicUserInfoFileName = $"userInfo.txt";
        private static string expensesInfoFileName = "userExpenses.txt";
        private static string incomesInfoFileName = "userIncomes.txt";
        private static string wishlistInfoFileName = "userWishlist.txt";

        private static CultureInfo culture = new CultureInfo("EN-US");
        private static long transactionId;

        public static long GetTransactionId()
        {
            return ++transactionId;
        }

        public static string GetSecretInput(string prompt)
        {
            bool isPrompt = true;
            string asterics = string.Empty;

            StringBuilder input = new StringBuilder();

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
            return Console.ReadLine();
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
            return String.Format(culture, "{0:C2}", amt);
        }

        public static UserAccount LoadUserInformation(UserAccount userAccount)
        {
            string userAccountInfoPath = $"{userAccount.Directory}/{basicUserInfoFileName}";
            string expensesPath = $"{userAccount.Directory}/{expensesInfoFileName}";
            string incomesPath = $"{userAccount.Directory}/{incomesInfoFileName}";
            string wishlistPath = $"{userAccount.Directory}/{wishlistInfoFileName}";

            userAccount = new UserAccount();

            if (File.Exists(userAccountInfoPath))
            {
                string[] userAccountInfoAsString = File.ReadAllLines(userAccountInfoPath);
                string[] userAccountInfoSplits = new string[6];

                for(int i = 0; i < userAccountInfoAsString.Length; i++)
                {
                    userAccountInfoSplits = userAccountInfoAsString[i].Split(';');
                }

                string fullName = userAccountInfoSplits[0].Substring(userAccountInfoSplits[0].IndexOf(':') + 1);
                string passcode = userAccountInfoSplits[1].Substring(userAccountInfoSplits[1].IndexOf(':') + 1);
                int id = int.Parse(userAccountInfoSplits[2].Substring(userAccountInfoSplits[2].IndexOf(':') + 1));
                bool isLocked = bool.Parse(userAccountInfoSplits[3].Substring(userAccountInfoSplits[3].IndexOf(':') + 1));
                int totalLogin = int.Parse(userAccountInfoSplits[4].Substring(userAccountInfoSplits[4].IndexOf(':') + 1));
                decimal balance = decimal.Parse(userAccountInfoSplits[5].Substring(userAccountInfoSplits[5].IndexOf(':') + 1));

                userAccount.FullName = fullName;
                userAccount.Passcode = passcode;
                userAccount.Id = id;
                userAccount.IsLocked = isLocked;
                userAccount.TotalLogin = totalLogin;
                userAccount.Balance = balance;
            }
            if (File.Exists(expensesPath))
            {
                string[] expensesAsString = File.ReadAllLines(expensesPath);
                string[] expensesSplits = new string[6];

                for (int i = 0; i < expensesAsString.Length; i++)
                {
                    expensesSplits = expensesAsString[i].Split(';');
                }
                if (expensesAsString.Any())
                {
                    string expenseName = expensesSplits[0].Substring(expensesSplits[0].IndexOf(':') + 1);
                    decimal amount = decimal.Parse(expensesSplits[1].Substring(expensesSplits[1].IndexOf(':') + 1));
                    int day = int.Parse(expensesSplits[2].Substring(expensesSplits[2].IndexOf(':') + 1));
                    int month = int.Parse(expensesSplits[3].Substring(expensesSplits[3].IndexOf(':') + 1));
                    int rate = int.Parse(expensesSplits[4].Substring(expensesSplits[4].IndexOf(':') + 1));
                    int id = int.Parse(expensesSplits[5].Substring(expensesSplits[5].IndexOf(':') + 1));

                    userAccount.ExpenseList.Add(new Expense() { ExpenseName = expenseName });
                    userAccount.ExpenseList.Add(new Expense() { Amount = amount });
                    userAccount.ExpenseList.Add(new Expense() { Day = day });
                    userAccount.ExpenseList.Add(new Expense() { Month = month });
                    userAccount.ExpenseList.Add(new Expense() { Rate = (Domain.Enums.Rate)rate });
                    userAccount.ExpenseList.Add(new Expense() { Id = id });
                }
            }
            if (File.Exists(incomesPath))
            {
                string[] incomesAsString = File.ReadAllLines(incomesPath);
                string[] incomesSplits = new string[6];

                for (int i = 0; i < incomesAsString.Length; i++)
                {
                    incomesSplits = incomesAsString[i].Split(';');
                
                    string incomeName = incomesSplits[0].Substring(incomesSplits[0].IndexOf(':') + 1);
                    decimal amount = decimal.Parse(incomesSplits[1].Substring(incomesSplits[1].IndexOf(':') + 1));
                    int day = int.Parse(incomesSplits[2].Substring(incomesSplits[2].IndexOf(':') + 1));
                    int month = int.Parse(incomesSplits[3].Substring(incomesSplits[3].IndexOf(':') + 1));
                    Domain.Enums.Rate rate = (Domain.Enums.Rate)int.Parse(incomesSplits[4].Substring(incomesSplits[4].IndexOf(':') + 1));
                    int id = int.Parse(incomesSplits[5].Substring(incomesSplits[5].IndexOf(':') + 1));

                    userAccount.IncomeList.Add(new Income() { IncomeName = incomeName,
                        Amount = amount,
                        Day = day,
                        Month = month,
                        Rate = rate,
                        Id = id
                    });
                    //userAccount.IncomeList.Add(new Income() { Amount = amount });
                    //userAccount.IncomeList.Add(new Income() { Day = day });
                    //userAccount.IncomeList.Add(new Income() { Month = month });
                    //userAccount.IncomeList.Add(new Income() { Rate = rate });
                    //userAccount.IncomeList.Add(new Income() { Id = id });
                }
            }
            if (File.Exists(wishlistPath))
            {
                string[] wishlistAsString = File.ReadAllLines(wishlistPath);
                string[] wishlistSplits = new string[6];

                for (int i = 0; i < wishlistAsString.Length; i++)
                {
                    wishlistSplits = wishlistAsString[i].Split(';');
                }

                if (wishlistAsString.Any())
                {
                    string item = wishlistSplits[0].Substring(wishlistSplits[0].IndexOf(':') + 1);
                    decimal cost = decimal.Parse(wishlistSplits[1].Substring(wishlistSplits[1].IndexOf(':') + 1));
                    int id = int.Parse(wishlistSplits[2].Substring(wishlistSplits[2].IndexOf(':') + 1));
                    int priority = int.Parse(wishlistSplits[3].Substring(wishlistSplits[3].IndexOf(':') + 1));

                    userAccount.Wishlist.Items.Add(new WishlistItem() { Item = item });
                    userAccount.Wishlist.Items.Add(new WishlistItem() { Cost = cost });
                    userAccount.Wishlist.Items.Add(new WishlistItem() { Id = id });
                    userAccount.Wishlist.Items.Add(new WishlistItem() { Priority = priority });
                }
            }
            return userAccount;
        }

        public static void SaveUserInformation(UserAccount userAccount)
        {
            string userInfoPath = $"{userAccount.Directory}/{basicUserInfoFileName}";
            StringBuilder userInfoSB = new StringBuilder();
            string expensesPath = $"{userAccount.Directory}/{expensesInfoFileName}";
            StringBuilder expensesSB = new StringBuilder();
            string incomesPath = $"{userAccount.Directory}/{incomesInfoFileName}";
            StringBuilder incomesSB = new StringBuilder();
            string wishlistPath = $"{userAccount.Directory}/{wishlistInfoFileName}";
            StringBuilder wishlistSB = new StringBuilder();  

            userInfoSB.Append($"fullName:{userAccount.FullName};");
            userInfoSB.Append($"passcode:{userAccount.Passcode.ToString()};");
            userInfoSB.Append($"id:{userAccount.Id};");
            userInfoSB.Append($"isLocked:{userAccount.IsLocked};");
            userInfoSB.Append($"totalLogin:{userAccount.TotalLogin};");
            userInfoSB.Append($"balance:{userAccount.Balance};");
            userInfoSB.Append(Environment.NewLine);

            foreach(Expense e in userAccount.ExpenseList)
            {
                expensesSB.Append($"expenseName:{e.ExpenseName};");
                expensesSB.Append($"amount:{e.Amount};");
                expensesSB.Append($"day:{e.Day};");
                expensesSB.Append($"month:{e.Month};");
                expensesSB.Append($"rate:{(int)e.Rate};");
                expensesSB.Append($"id:{e.Id};");
                expensesSB.Append(Environment.NewLine);
            }
            foreach (Income i in userAccount.IncomeList)
            {
                incomesSB.Append($"incomeName:{i.IncomeName};");
                incomesSB.Append($"amount:{i.Amount};");
                incomesSB.Append($"day:{i.Day};");
                incomesSB.Append($"month:{i.Month};");
                incomesSB.Append($"rate:{(int)i.Rate};");
                incomesSB.Append($"id:{i.Id};");
                incomesSB.Append(Environment.NewLine);
            }
            foreach (WishlistItem w in userAccount.Wishlist.Items)
            {
                wishlistSB.Append($"itemName:{w.Item};");
                wishlistSB.Append($"cost:{w.Cost};");
                wishlistSB.Append($"id:{w.Id};");
                wishlistSB.Append($"priority:{w.Priority};");
                wishlistSB.Append(Environment.NewLine);
            }

            //sb.Append("#wishlist");
            //sb.Append(Environment.NewLine);
            //for (int i = 0; i < userAccount.Wishlist.Items.Count; i++)
            //{
            //    WishlistItem wishlistItem = userAccount.Wishlist.Items[i];
            //    sb.Append($"wishListItem:{wishlistItem.Item};");
            //    sb.Append($"wishListItemCost:{wishlistItem.Cost};");
            //    sb.Append($"wishListItemId:{wishlistItem.Id};");
            //    sb.Append($"wishListItemPriority:{wishlistItem.Priority};");

            //    sb.Append(Environment.NewLine);
            //}

            //sb.Append("#expenses");
            //sb.Append(Environment.NewLine);
            //for (int i = 0; i < userAccount.ExpenseList.Count; i++)
            //{
            //    Expense expense = userAccount.ExpenseList[i];
            //    sb.Append($"expenseName:{expense.ExpenseName};");
            //    sb.Append($"expenseAmount:{expense.Amount};");
            //    sb.Append($"expenseDay:{expense.Day};");
            //    sb.Append($"expenseId:{expense.Id};");
            //    sb.Append($"expenseMonth:{expense.Month};");
            //    sb.Append($"expenseRate:{expense.Rate};");

            //    sb.Append(Environment.NewLine);
            //}

            //sb.Append("#incomes");
            //sb.Append(Environment.NewLine);
            //for (int i = 0; i < userAccount.IncomeList.Count; i++)
            //{
            //    Income income = userAccount.IncomeList[i];
            //    sb.Append($"inocomeName:{income.IncomeName};");
            //    sb.Append($"inocomeAmount:{income.Amount};");
            //    sb.Append($"inocomeDay:{income.Day};");
            //    sb.Append($"inocomeId:{income.Id};");
            //    sb.Append($"inocomeMonth:{income.Month};");
            //    sb.Append($"inocomeRate:{income.Rate};");

            //    sb.Append(Environment.NewLine);
            //}

            File.WriteAllText(userInfoPath, userInfoSB.ToString());
            File.WriteAllText(expensesPath, expensesSB.ToString());
            File.WriteAllText(incomesPath, incomesSB.ToString());
            File.WriteAllText(wishlistPath, wishlistSB.ToString());
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Saved user info successfully!");
            Console.ResetColor();
            
        }

        public static void CheckForExistingUserFile(UserAccount userAccount)
        {
            string basicUserInfoPath = $"{userAccount.Directory}{basicUserInfoFileName}.txt";
            bool existingBasicUserInfoFileFound = File.Exists(basicUserInfoPath);
            string expensesPath = $"{userAccount.Directory}{expensesInfoFileName}";
            bool existingExpensesFileFound = File.Exists(expensesPath);
            string incomesPath = $"{userAccount.Directory}{incomesInfoFileName}";
            bool existingIncomesFileFound = File.Exists(incomesPath);
            string wishlistPath = $"{userAccount.Directory}{wishlistInfoFileName}";
            bool existingWishlistFileFound = File.Exists(wishlistPath);
            if (existingBasicUserInfoFileFound)
            {
                Console.WriteLine("Existing file for basic user info found");
            }
            
            if (existingExpensesFileFound)
            {
                Console.WriteLine("Existing file for expenses found");
            }
            if (existingIncomesFileFound)
            {
                Console.WriteLine("Existing file for incomes found");
            }

            if (existingWishlistFileFound)
            {
                Console.WriteLine("Existing file for wishlist found");
            }
            if (!Directory.Exists(userAccount.Directory))
            {
                Directory.CreateDirectory(userAccount.Directory);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Directory is ready for saving expenses info files");
                Console.ResetColor();
            }
        }
    }
}

