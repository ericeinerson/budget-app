using System.Globalization;
using System.Text;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;

namespace BudgetApp.UI
{
	public static class Utilities
	{
        private static readonly CultureInfo culture = new("EN-US");
        private static long transactionId;

        public static long GetTransactionId()
        {
            return ++transactionId;
        }

        public static string GetSecretInput(string prompt)
        {
            var isPrompt = true;
            var asterics = string.Empty;

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
            var directory = userAccount.Directory;
            var userInfoFilePath = Path.Combine(userAccount.Directory, userAccount.UserInfoFileName);
            var expensesFilePath = Path.Combine(userAccount.Directory, userAccount.ExpensesFileName);
            var incomesFilePath = Path.Combine(userAccount.Directory, userAccount.IncomesFileName);
            var transactionsFilePath = Path.Combine(userAccount.Directory, userAccount.TransactionsFileName);
            var categoriesFilePath = Path.Combine(userAccount.Directory, userAccount.CategoriesFileName);
            var wishlistFilePath = Path.Combine(userAccount.Directory, userAccount.WishlistFileName);

            userAccount = new UserAccount()
            {
                Directory = directory,
                UserInfoFileName = userInfoFilePath,
                ExpensesFileName = expensesFilePath,
                IncomesFileName = incomesFilePath,
                TransactionsFileName = transactionsFilePath,
                CategoriesFileName = categoriesFilePath,
                WishlistFileName = wishlistFilePath
            };

            var isLoaded = false;

            if (File.Exists(userInfoFilePath))
            {
                string[] userAccountInfoAsString = File.ReadAllLines(userInfoFilePath);
                var userAccountInfoSplits = new List<string>();

                for (int i = 0; i < userAccountInfoAsString.Length; i++)
                {
                    userAccountInfoSplits = userAccountInfoAsString[i].Split(';').ToList();
                }
                var id = userAccountInfoSplits[0][(userAccountInfoSplits[0].IndexOf(':') + 1)..] == "null" ? -1 : int.Parse(userAccountInfoSplits[0][(userAccountInfoSplits[0].IndexOf(':') + 1)..]);
                var fullName = userAccountInfoSplits[1][(userAccountInfoSplits[1].IndexOf(':') + 1)..];
                var passcode = userAccountInfoSplits[2][(userAccountInfoSplits[2].IndexOf(':') + 1)..];
                var isLocked = bool.Parse(userAccountInfoSplits[3].AsSpan(userAccountInfoSplits[3].IndexOf(':') + 1));
                var totalLogin = int.Parse(userAccountInfoSplits[4][(userAccountInfoSplits[4].IndexOf(':') + 1)..]);
                var balance = decimal.Parse(userAccountInfoSplits[5][(userAccountInfoSplits[5].IndexOf(':') + 1)..]);
                var incomeId = int.Parse(userAccountInfoSplits[7][(userAccountInfoSplits[7].IndexOf(':') + 1)..]);
                var expenseId = int.Parse(userAccountInfoSplits[8][(userAccountInfoSplits[8].IndexOf(':') + 1)..]);
                var wishlistId = int.Parse(userAccountInfoSplits[9][(userAccountInfoSplits[9].IndexOf(':') + 1)..]);
                var budgetItemId = int.Parse(userAccountInfoSplits[10][(userAccountInfoSplits[10].IndexOf(':') + 1)..]);
                var transactionId = int.Parse(userAccountInfoSplits[11][(userAccountInfoSplits[11].IndexOf(':') + 1)..]);
                var lastLoginDate = DateTime.Parse(userAccountInfoSplits[12][(userAccountInfoSplits[12].IndexOf(':') + 1)..]);

                userAccount.Id = id;
                userAccount.FullName = fullName;
                userAccount.Passcode = passcode;
                userAccount.IsLocked = isLocked;
                userAccount.TotalLogin = totalLogin;
                userAccount.Balance = balance;
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

            if (File.Exists(expensesFilePath))
            {
                string[] expensesAsString = File.ReadAllLines(expensesFilePath);

                //LOOK INTO LINQ FOR DOING THIS
                for (int i = 0; i < expensesAsString.Length; i++)
                {
                    List<string>? expensesSplits = expensesAsString[i].Split(';').ToList();

                    var id = int.Parse(expensesSplits[0].Substring(expensesSplits[0].IndexOf(':') + 1));
                    var expenseId = int.Parse(expensesSplits[1].Substring(expensesSplits[1].IndexOf(':') + 1));
                    var categoryId = int.Parse(expensesSplits[2].Substring(expensesSplits[2].IndexOf(':') + 1));
                    var expenseName = expensesSplits[3].Substring(expensesSplits[3].IndexOf(':') + 1);
                    var amount = decimal.Parse(expensesSplits[4].Substring(expensesSplits[4].IndexOf(':') + 1));
                    DateTime? startDate = expensesSplits[5].Substring(expensesSplits[5].IndexOf(':') + 1) == "null" ? null : DateTime.Parse(expensesSplits[5].Substring(expensesSplits[5].IndexOf(':') + 1));
                    DateTime? endDate = expensesSplits[6].Substring(expensesSplits[6].IndexOf(':') + 1) == "null" ? null : DateTime.Parse(expensesSplits[6].Substring(expensesSplits[6].IndexOf(':') + 1));
                    var rate = (Rate)int.Parse(expensesSplits[7].Substring(expensesSplits[7].IndexOf(':') + 1));
                    var amountVariable = bool.Parse(expensesSplits[8].Substring(expensesSplits[8].IndexOf(':') + 1));

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
                        AmountVariable = amountVariable,
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
            if (File.Exists(incomesFilePath))
            {

                string[] incomesAsString = File.ReadAllLines(incomesFilePath);

                for (int i = 0; i < incomesAsString.Length; i++)
                {
                    List<string>? incomesSplits = incomesAsString[i].Split(';').ToList();

                    var id = int.Parse(incomesSplits[0].Substring(incomesSplits[0].IndexOf(':') + 1));
                    var incomeId = int.Parse(incomesSplits[1].Substring(incomesSplits[1].IndexOf(':') + 1));
                    var categoryId = int.Parse(incomesSplits[2].Substring(incomesSplits[2].IndexOf(':') + 1));
                    var incomeName = incomesSplits[3].Substring(incomesSplits[3].IndexOf(':') + 1);
                    var amount = decimal.Parse(incomesSplits[4].Substring(incomesSplits[4].IndexOf(':') + 1));
                    DateTime? startDate = incomesSplits[5].Substring(incomesSplits[5].IndexOf(':') + 1) == "null" ? null : DateTime.Parse(incomesSplits[5].Substring(incomesSplits[5].IndexOf(':') + 1));
                    DateTime? endDate = incomesSplits[6].Substring(incomesSplits[6].IndexOf(':') + 1) == "null" ? null : DateTime.Parse(incomesSplits[6].Substring(incomesSplits[6].IndexOf(':') + 1));
                    var rate = (Rate)int.Parse(incomesSplits[7].Substring(incomesSplits[7].IndexOf(':') + 1));
                    var amountVariable = bool.Parse(incomesSplits[8].Substring(incomesSplits[8].IndexOf(':') + 1));

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
                        AmountVariable = amountVariable,
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
            if (File.Exists(transactionsFilePath))
            {
                string[] transactionsAsString = File.ReadAllLines(transactionsFilePath);

                for (int i = 0; i < transactionsAsString.Length; i++)
                {
                    List<string>? transactionsSplits = transactionsAsString[i].Split(';').ToList();

                    var id = int.Parse(transactionsSplits[0][(transactionsSplits[0].IndexOf(':') + 1)..]);
                    var categoryId = int.Parse(transactionsSplits[1][(transactionsSplits[1].IndexOf(':') + 1)..]);
                    var budgetItemId = int.Parse(transactionsSplits[2][(transactionsSplits[2].IndexOf(':') + 1)..]);
                    var transactionName = transactionsSplits[3][(transactionsSplits[3].IndexOf(':') + 1)..];
                    var amount = decimal.Parse(transactionsSplits[4][(transactionsSplits[4].IndexOf(':') + 1)..]);
                    var createdDate = DateTime.Parse(transactionsSplits[5][(transactionsSplits[5].IndexOf(':') + 1)..]);
                    DateTime? postedDate = transactionsSplits[6][(transactionsSplits[6].IndexOf(':') + 1)..] == "null" ? null : DateTime.Parse(transactionsSplits[7][(transactionsSplits[7].IndexOf(':') + 1)..]);
                    var scheduledDate = DateTime.Parse(transactionsSplits[7][(transactionsSplits[7].IndexOf(':') + 1)..]);
                    var type = (BudgetItemType)int.Parse(transactionsSplits[8].Substring(transactionsSplits[8].IndexOf(':') + 1));
                    var status = (Status)int.Parse(transactionsSplits[9][(transactionsSplits[9].IndexOf(':') + 1)..]);

                    userAccount.TransactionList.Add(new Transaction()
                    {
                        Id = id,
                        CategoryId = categoryId,
                        BudgetItemId = budgetItemId,
                        Name = transactionName,
                        Amount = amount,
                        CreatedDate = createdDate,
                        ScheduledDate = scheduledDate,
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
            if (File.Exists(categoriesFilePath))
            {
                string[] categoriesAsString = File.ReadAllLines(categoriesFilePath);

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
            if (File.Exists(wishlistFilePath))
            {
                string[] wishlistAsString = File.ReadAllLines(wishlistFilePath);

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
            var userInfoPath = Path.Combine(userAccount.Directory, userAccount.UserInfoFileName);
            StringBuilder userInfoSB = new();
            var expensesPath = Path.Combine(userAccount.Directory, userAccount.ExpensesFileName);
            StringBuilder expensesSB = new();
            var incomesPath = Path.Combine(userAccount.Directory, userAccount.IncomesFileName);
            StringBuilder incomesSB = new();
            var transactionsPath = Path.Combine(userAccount.Directory, userAccount.TransactionsFileName);
            StringBuilder transactionsSB = new();
            var categoriesPath = Path.Combine(userAccount.Directory, userAccount.CategoriesFileName);
            StringBuilder categoriesSB = new();
            var wishlistPath = Path.Combine(userAccount.Directory, userAccount.WishlistFileName);
            StringBuilder wishlistSB = new();

            ConstructUserInfoStringBuilder(userAccount, userInfoSB);
            ConstructExpensesStringBuilder(userAccount, expensesSB);
            ConstructIncomesStringBuilder(userAccount, incomesSB);
            ConstructTransactionsStringBuilder(userAccount, transactionsSB);
            ConstructCategoriesStringBuilder(userAccount, categoriesSB);
            ConstructWishlistStringBuilder(userAccount, wishlistSB);

            File.WriteAllText(userInfoPath, userInfoSB.ToString());
            File.WriteAllText(expensesPath, expensesSB.ToString());
            File.WriteAllText(incomesPath, incomesSB.ToString());
            File.WriteAllText(transactionsPath, transactionsSB.ToString());
            File.WriteAllText(categoriesPath, categoriesSB.ToString());
            File.WriteAllText(wishlistPath, wishlistSB.ToString());
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Saved user info successfully!");
        }

        private static void ConstructUserInfoStringBuilder(UserAccount userAccount, StringBuilder userInfoSB)
        {
            var id = string.IsNullOrEmpty(userAccount.Id.ToString()) ? "null" : userAccount.Id.ToString();
            var fullName = string.IsNullOrEmpty(userAccount.FullName.ToString()) ? "null" : userAccount.FullName.ToString();
            var passcode = string.IsNullOrEmpty(userAccount.Passcode.ToString()) ? "null" : userAccount.Passcode.ToString();
            var isLocked = string.IsNullOrEmpty(userAccount.IsLocked.ToString()) ? "null" : userAccount.IsLocked.ToString();
            var totalLogin = string.IsNullOrEmpty(userAccount.TotalLogin.ToString()) ? "null" : userAccount.TotalLogin.ToString();
            var balance = string.IsNullOrEmpty(userAccount.Balance.ToString()) ? "null" : userAccount.Balance.ToString();
            var directory = string.IsNullOrEmpty(userAccount.Directory.ToString()) ? "null" : userAccount.Directory.ToString();
            var incomeIdCounter = string.IsNullOrEmpty(userAccount.IncomeIdCounter.ToString()) ? "null" : userAccount.IncomeIdCounter.ToString();
            var expenseIdCounter = string.IsNullOrEmpty(userAccount.ExpenseIdCounter.ToString()) ? "null" : userAccount.ExpenseIdCounter.ToString();
            var wishlistIdCounter = string.IsNullOrEmpty(userAccount.WishlistIdCounter.ToString()) ? "null" : userAccount.WishlistIdCounter.ToString();
            var budgetItemIdCounter = string.IsNullOrEmpty(userAccount.BudgetItemIdCounter.ToString()) ? "null" : userAccount.BudgetItemIdCounter.ToString();
            var transactionIdCounter = string.IsNullOrEmpty(userAccount.TransactionIdCounter.ToString()) ? "null" : userAccount.TransactionIdCounter.ToString();
            var lastLoginDate = string.IsNullOrEmpty(userAccount.LastLoginDate.ToString()) ? "null" : userAccount.LastLoginDate.ToString();

            userInfoSB.Append($"id:{id};");//0
            userInfoSB.Append($"fullName:{fullName};");//1
            userInfoSB.Append($"passcode:{passcode};");//2
            userInfoSB.Append($"isLocked:{isLocked};");//3
            userInfoSB.Append($"totalLogin:{totalLogin};");//4
            userInfoSB.Append($"balance:{balance};");//5
            userInfoSB.Append($"directory:{directory};");//6
            userInfoSB.Append($"income id:{incomeIdCounter};");//7
            userInfoSB.Append($"expense id:{expenseIdCounter};");//8
            userInfoSB.Append($"wishlist id:{wishlistIdCounter};");//9
            userInfoSB.Append($"budget item id:{budgetItemIdCounter};");//10
            userInfoSB.Append($"transaction id:{transactionIdCounter};");//11
            userInfoSB.Append($"last login date:{lastLoginDate};");//12
            userInfoSB.Append(Environment.NewLine);
        }

        private static void ConstructWishlistStringBuilder(UserAccount userAccount, StringBuilder wishlistSB)
        {
            foreach (WishlistItem w in userAccount.Wishlist.Items)
            {
                var id = string.IsNullOrEmpty(w.Id.ToString()) ? "null" : w.Id.ToString();
                var item = string.IsNullOrEmpty(w.Item.ToString()) ? "null" : w.Item.ToString();
                var cost = string.IsNullOrEmpty(w.Cost.ToString()) ? "null" : w.Cost.ToString();
                var priority = string.IsNullOrEmpty(w.Priority.ToString()) ? "null" : w.Priority.ToString();

                wishlistSB.Append($"id:{id};"); //0
                wishlistSB.Append($"wishlist name:{item};"); //1
                wishlistSB.Append($"cost:{cost};"); //2
                wishlistSB.Append($"priority:{priority};"); //3
                wishlistSB.Append(Environment.NewLine);
            }
            foreach (WishlistItem w in userAccount.Wishlist.Items)
            {
                wishlistSB.Append($"id:{w.Id};");//0
                wishlistSB.Append($"itemName:{w.Item};");//1
                wishlistSB.Append($"cost:{w.Cost};");//2
                wishlistSB.Append($"priority:{w.Priority};");//3
                wishlistSB.Append(Environment.NewLine);
            }
        }

        private static void ConstructCategoriesStringBuilder(UserAccount userAccount, StringBuilder categoriesSB)
        {
            foreach (Category c in userAccount.CategoryList)
            {
                var id = string.IsNullOrEmpty(c.Id.ToString()) ? "null" : c.Id.ToString();
                var name = string.IsNullOrEmpty(c.Name.ToString()) ? "null" : c.Name.ToString();

                categoriesSB.Append($"id:{id};"); //0
                categoriesSB.Append($"category name:{name};"); //1
                categoriesSB.Append(Environment.NewLine);
            }
        }

        private static void ConstructTransactionsStringBuilder(UserAccount userAccount, StringBuilder transactionsSB)
        {
            foreach (Transaction t in userAccount.TransactionList)
            {
                var id = string.IsNullOrEmpty(t.Id.ToString()) ? "null" : t.Id.ToString();
                var categoryId = string.IsNullOrEmpty(t.CategoryId.ToString()) ? "null" : t.CategoryId.ToString();
                var budgetItemId = string.IsNullOrEmpty(t.BudgetItemType.ToString()) ? "null" : t.BudgetItemId.ToString();
                var name = string.IsNullOrEmpty(t.Name.ToString()) ? "null" : t.Name.ToString();
                var amount = string.IsNullOrEmpty(t.Amount.ToString()) ? "null" : t.Amount.ToString();
                var createdDate = string.IsNullOrEmpty(t.CreatedDate.ToString()) ? "null" : t.CreatedDate.ToString();
                var postedDate = string.IsNullOrEmpty(t.PostedDate.ToString()) ? "null" : t.PostedDate.ToString();
                var scheduledDate = string.IsNullOrEmpty(t.ScheduledDate.ToString()) ? "null" : t.ScheduledDate.ToString();
                var budgetItemType = string.IsNullOrEmpty(t.BudgetItemType.ToString()) ? "null" : ((int)t.BudgetItemType).ToString();
                var status = string.IsNullOrEmpty(t.Status.ToString()) ? "null" : ((int)t.Status).ToString();

                transactionsSB.Append($"id:{id};"); //0
                transactionsSB.Append($"category id:{categoryId};"); //1
                transactionsSB.Append($"budget item id:{budgetItemId};"); //2
                transactionsSB.Append($"transaction name:{name};");//3
                transactionsSB.Append($"amount:{amount};");//4
                transactionsSB.Append($"created date:{createdDate};");//5
                transactionsSB.Append($"posted date:{postedDate};");//6
                transactionsSB.Append($"scheduled date:{scheduledDate};");//7
                transactionsSB.Append($"budget item type:{budgetItemType};");//8
                transactionsSB.Append($"status:{status};");//9
                transactionsSB.Append(Environment.NewLine);
            }
        }

        private static void ConstructIncomesStringBuilder(UserAccount userAccount, StringBuilder incomesSB)
        {
            foreach (Income i in userAccount.IncomeList.Cast<Income>())
            {
                var id = string.IsNullOrEmpty(i.Id.ToString()) ? "null" : i.Id.ToString();
                var incomeId = string.IsNullOrEmpty(i.IncomeId.ToString()) ? "null" : i.IncomeId.ToString();
                var categoryId = string.IsNullOrEmpty(i.CategoryId.ToString()) ? "null" : i.CategoryId.ToString();
                var name = string.IsNullOrEmpty(i.Name.ToString()) ? "null" : i.Name.ToString();
                var amount = string.IsNullOrEmpty(i.Amount.ToString()) ? "null" : i.Amount.ToString();
                var startDate = string.IsNullOrEmpty(i.StartDate.ToString()) ? "null" : i.StartDate.ToString();
                var endDate = string.IsNullOrEmpty(i.EndDate.ToString()) ? "null" : i.EndDate.ToString();
                var rate = string.IsNullOrEmpty(i.Rate.ToString()) ? "null" : ((int)i.Rate).ToString();
                var amountVariable = string.IsNullOrEmpty(i.AmountVariable.ToString()) ? "null" : i.AmountVariable.ToString();

                incomesSB.Append($"id:{id};"); //0
                incomesSB.Append($"incomeId:{incomeId};"); //1
                incomesSB.Append($"category id:{categoryId};"); //2
                incomesSB.Append($"incomeName:{name};");//3
                incomesSB.Append($"amount:{amount};");//4
                incomesSB.Append($"start date:{startDate};");//5
                incomesSB.Append($"end date:{endDate};");//6
                incomesSB.Append($"rate:{rate};");//7
                incomesSB.Append($"amount variable:{amountVariable};");//8
                incomesSB.Append(Environment.NewLine);
            }
        }

        private static void ConstructExpensesStringBuilder(UserAccount userAccount, StringBuilder expensesSB)
        {
            foreach (Expense e in userAccount.ExpenseList.Cast<Expense>())
            {
                var id = string.IsNullOrEmpty(e.Id.ToString()) ? "null" : e.Id.ToString();
                var expenseId = string.IsNullOrEmpty(e.ExpenseId.ToString()) ? "null" : e.ExpenseId.ToString();
                var categoryId = string.IsNullOrEmpty(e.CategoryId.ToString()) ? "null" : e.CategoryId.ToString();
                var name = string.IsNullOrEmpty(e.Name.ToString()) ? "null" : e.Name.ToString();
                var amount = string.IsNullOrEmpty(e.Amount.ToString()) ? "null" : e.Amount.ToString();
                var startDate = string.IsNullOrEmpty(e.StartDate.ToString()) ? "null" : e.StartDate.ToString();
                var endDate = string.IsNullOrEmpty(e.EndDate.ToString()) ? "null" : e.EndDate.ToString();
                var rate = string.IsNullOrEmpty(e.Rate.ToString()) ? "null" : ((int)e.Rate).ToString();
                var amountVariable = string.IsNullOrEmpty(e.AmountVariable.ToString()) ? "null" : e.AmountVariable.ToString();

                expensesSB.Append($"id:{id};"); //0
                expensesSB.Append($"expenseId:{expenseId};"); //1
                expensesSB.Append($"category id:{categoryId};"); //2
                expensesSB.Append($"expense name:{name};");//3
                expensesSB.Append($"amount:{amount};");//4
                expensesSB.Append($"start date:{startDate};");//5
                expensesSB.Append($"end date:{endDate};");//6
                expensesSB.Append($"rate:{rate};");//7
                expensesSB.Append($"amount variable:{amountVariable};");//8
                expensesSB.Append(Environment.NewLine);
            }
        }

        public static void SaveUserInfoOnlyWithNewLoginTime(UserAccount userAccount)
        {
            DateTime loginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            userAccount.LastLoginDate = loginDate;

            string userInfoPath = Path.Combine(userAccount.Directory, "userInfo.txt");
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
            var directoryPath = new DirectoryInfo(".").Parent?.Parent?.Parent?.Parent?.Parent?.FullName ?? "";
            userAccount.UserInfoFileName = "userInfo.txt";
            userAccount.ExpensesFileName = "expenses.txt";
            userAccount.IncomesFileName = "incomes.txt";
            userAccount.TransactionsFileName = "transactions.txt";
            userAccount.CategoriesFileName = "categories.txt";
            userAccount.WishlistFileName = "wishlist.txt";

            var promptWhichDirectory = PromptYesOrNo("Would you like to save/load data from a different directory/file structure than the default?");

            if (promptWhichDirectory == "y")
            {
                userAccount.Directory = GetUserInput("new directory to use for loading data");
                userAccount.UserInfoFileName = GetUserInput("new user info file name to use for loading data");
                userAccount.ExpensesFileName = GetUserInput("new expenses file name to use for loading data");
                userAccount.IncomesFileName = GetUserInput("new incomes file name to use for loading data");
                userAccount.TransactionsFileName = GetUserInput("new transactions file name to use for loading data");
                userAccount.CategoriesFileName = GetUserInput("new categories file name to use for loading data");
                userAccount.WishlistFileName = GetUserInput("new wishlist file name to use for loading data");
            }
            else
            {
                directoryPath = Path.Combine(directoryPath, "BudgetAppProject_UserInfo");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                userAccount.Directory = directoryPath;
            }

            var promptFolder = PromptYesOrNo("Would you like to choose a folder in your directory to load data from?");
            if (promptFolder == "y")
            {
                var directoryFolderToUse = GetUserInput("folder name");
                userAccount.Directory = Path.Combine(userAccount.Directory, directoryFolderToUse);
            }
            bool isSavedData = false;
            string basicUserInfoPath = Path.Combine(userAccount.Directory, userAccount.UserInfoFileName); ;
            bool existingBasicUserInfoFileFound = File.Exists(basicUserInfoPath);
            string expensesPath = Path.Combine(userAccount.Directory,userAccount.ExpensesFileName);
            bool existingExpensesFileFound = File.Exists(expensesPath);
            string incomesPath = Path.Combine(userAccount.Directory, userAccount.IncomesFileName);
            bool existingIncomesFileFound = File.Exists(incomesPath);
            string transactionsPath = Path.Combine(userAccount.Directory, userAccount.TransactionsFileName);
            bool existingTransactionnsFileFount = File.Exists(transactionsPath);
            string wishlistPath = Path.Combine(userAccount.Directory, userAccount.WishlistFileName);
            bool existingWishlistFileFound = File.Exists(wishlistPath);
            string categoryPath = Path.Combine(userAccount.Directory, userAccount.CategoriesFileName);
            bool existingCategoryFileFound = File.Exists(categoryPath);

            if (existingBasicUserInfoFileFound || existingExpensesFileFound || existingIncomesFileFound || existingTransactionnsFileFount || existingCategoryFileFound || existingWishlistFileFound)
            {
                isSavedData = true;
            }
            {
                Console.WriteLine("Existing file for basic user info found");
            }
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
            if (existingTransactionnsFileFount)
            {
                Console.WriteLine("Existing file for transactions found");
            }
            if (existingWishlistFileFound)
            {
                Console.WriteLine("Existing file for wishlist found");
            }
            if (existingCategoryFileFound)
            {
                Console.WriteLine("Existing file for categories found");
            }

            if (!Directory.Exists(userAccount.Directory))
            {
                string prompt = PromptYesOrNo("No directory found. Would you like to create a directory?");
                if (prompt == "y")
                {
                    var projectPath = new DirectoryInfo(".").Parent?.Parent?.Parent?.FullName ?? "";
                    var useDefaultDirectory = GetUserInput("Use default directory location?");
                    if (useDefaultDirectory == "n")
                    {
                        projectPath = new DirectoryInfo(GetUserInput("the directory path you'd like to save files in")).FullName;
                    }
                    
                    userAccount.Directory = projectPath;
                    TestDirectory(userAccount);

                    var nameFilesPrompt = PromptYesOrNo("Would you like to give your files a custom name?");
                    if(nameFilesPrompt == "y")
                    {
                        userAccount.UserInfoFileName = GetUserInput("User info file name");
                        userAccount.ExpensesFileName = GetUserInput("Expenses file name");
                        userAccount.IncomesFileName = GetUserInput("Incomes file name");
                        userAccount.TransactionsFileName = GetUserInput("Transactions file name");
                        userAccount.CategoriesFileName = GetUserInput("Categories file name");
                        userAccount.WishlistFileName = GetUserInput("Wishlist file name");
                    }
                    else
                    {
                        userAccount.UserInfoFileName = "userInfo.txt";
                        userAccount.ExpensesFileName = "expenses.txt";
                        userAccount.IncomesFileName = "incomes.txt";
                        userAccount.TransactionsFileName = "transactions.txt";
                        userAccount.CategoriesFileName = "categories.txt";
                        userAccount.WishlistFileName = "wishlist.txt";
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Directory is ready for saving info files");
                    Console.ResetColor();
                }
                else if (prompt == "n")
                {
                    return false;
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

        public static bool RateIsInRange(int value)
        {
            var values = Enum.GetValues(typeof(Rate)).Cast<int>().OrderBy(x => x);

            return value >= values.First() && value <= values.Last();
        }

        public static void TestDirectory(UserAccount userAccount)
        {
            var testFileName = "test.txt";
            var testStr = "test";
            var success = false;

            while (!success)
            {
                try
                {
                    File.WriteAllText(Path.Combine(userAccount.Directory, testFileName), testStr);
                    File.ReadAllText(Path.Combine(userAccount.Directory, testFileName));
                }
                catch
                {
                    PrintMessage("Invalid directory. Please try again", false, false);
                    userAccount.Directory = GetUserInput("the directory path you'd like to save files in");
                    Directory.CreateDirectory(userAccount.Directory);
                    continue;
                }
                success = true;
            }
            return;
        }
    }
}

