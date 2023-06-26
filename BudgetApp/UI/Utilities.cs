using System;
using System.Globalization;
using System.Text;
using BudgetApp.Domain.Entities;

namespace BudgetApp.UI
{
	public static class Utilities
	{
        private static string directory = @"/Users/ericeinerson/Projects/BudgetApp/UserInfo";
        private static string fileName = "userInfo.txt";
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
            string path = $"{directory}/{fileName}";
            userAccount = new UserAccount();
            if (File.Exists(path))
            {
                string[] userAccountInfoAsString = File.ReadAllLines(path);
                string[] userSplits = new string[6];

                for(int i = 0; i < userAccountInfoAsString.Length; i++)
                {
                    userSplits = userAccountInfoAsString[i].Split(';');
                }

                string fullName = userSplits[0].Substring(userSplits[0].IndexOf(':') + 1);
                string passcode = userSplits[1].Substring(userSplits[1].IndexOf(':') + 1);
                int id = int.Parse(userSplits[2].Substring(userSplits[2].IndexOf(':') + 1));
                bool isLocked = bool.Parse(userSplits[3].Substring(userSplits[3].IndexOf(':') + 1));
                int totalLogin = int.Parse(userSplits[4].Substring(userSplits[4].IndexOf(':') + 1));
                decimal balance = decimal.Parse(userSplits[5].Substring(userSplits[5].IndexOf(':') + 1));

                userAccount.FullName = fullName;
                userAccount.Passcode = passcode;
                userAccount.Id = id;
                userAccount.IsLocked = isLocked;
                userAccount.TotalLogin = totalLogin;
                userAccount.Balance = balance;
            }
            return userAccount;
        }

        public static void SaveUserInformation(UserAccount userAccount)
        {
            string path = $"{directory}/{fileName}";
            StringBuilder sb = new StringBuilder();

            sb.Append($"fullName:{userAccount.FullName};");

            sb.Append($"passcode:{userAccount.Passcode.ToString()};");

            sb.Append($"id:{userAccount.Id};");

            sb.Append($"isLocked:{userAccount.IsLocked};");

            sb.Append($"totalLogin:{userAccount.TotalLogin};");

            sb.Append($"balance:{userAccount.Balance};");
            sb.Append(Environment.NewLine);

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
  
                File.WriteAllText(path, sb.ToString());
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Saved expenses successfully!");
                Console.ResetColor();
            
        }

        public static void CheckForExistingUserFile()
        {
            string path = $"{directory}{fileName}";
            bool existingFileFound = File.Exists(path);

            if (existingFileFound)
            {
                Console.WriteLine("Existing file found");
            }
            else
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Directory is ready for saving files");
                    Console.ResetColor();
                }
            }
        }
    }
}

