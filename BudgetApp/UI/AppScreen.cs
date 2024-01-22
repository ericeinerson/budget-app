using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Extensions;
using BudgetApp.App;
using BudgetApp;

namespace BudgetApp.UI
{
	public class AppScreen
	{
        internal const string cur = "$";

        internal static void Welcome()
        {
            Console.Clear();
            Console.Title = "My Budget App";
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("\n\n------------Welcome to My Budget App------------\n\n");

            Utilities.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new()
            {
                FullName = Utilities.GetUserInput("your name.").ToLower(),
                Passcode = Utilities.GetSecretInput("Enter your passcode.")
            };

            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking name and passcode...");
            Utilities.PrintDotAnimation();
        }

        internal static void LogoutProgress()
        {
            Console.WriteLine("Thank you for using My Budget App.");
            Utilities.PrintDotAnimation();
            Console.Clear();
            string logoutOption = Utilities.PromptYesONo("Would you like to exit the app?").ToLower();

            if (logoutOption == "y")
            {
                Environment.Exit(1);
            }

            Utilities.PrintMessage("You have successfully logged out.", true);
            //var budgetApp = new BudgetApp.App.BudgetApp();
            //budgetApp.Run();
        }

        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utilities.PrintMessage("Your account is locked. Please go to the admin to unlock your account. Thank you.", true);

            Environment.Exit(1);
        }

        internal static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welcome back, {fullName}");
            Utilities.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("-------My Budget App Menu-------");
            foreach(AppMenu menuItem in Enum.GetValues<AppMenu>())
            {
                var menuDescription = menuItem.GetDescription();
                var menuLine = string.Format("{0}. {1}",(int)menuItem, menuDescription);

                Console.WriteLine(menuLine);
            }
        }

        internal static void DisplayExpenseOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an option. \n\n");

            foreach (ExpenseOption menuItem in Enum.GetValues<ExpenseOption>())
            {
                var menuDescription = menuItem.GetDescription();
                var menuLine = string.Format("{0}. {1}", (int)menuItem, menuDescription);

                Console.WriteLine(menuLine);
            }
        }

        internal static void DisplayCategoryOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an option. \n\n");

            foreach (CategoryOption menuItem in Enum.GetValues<CategoryOption>())
            {
                var menuDescription = menuItem.GetDescription();
                var menuLine = string.Format("{0}. {1}", (int)menuItem, menuDescription);

                Console.WriteLine(menuLine);
            }
        }

        internal static void DisplayExpenseUpdateOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an update option. \n\n");

            Console.WriteLine("1. Pay All/Remaining                        ");
            Console.WriteLine("2. Pay Partial                              ");
            Console.WriteLine("3. Enter new amount                         ");
            Console.WriteLine("4. Logout                                   ");
            Console.WriteLine("5. Add to expense list                      ");

        }

        internal static void DisplayExpenseUpdateDetails()
        {
            Console.Clear();
            Console.WriteLine("Select a detail to update. \n\n");

            Console.WriteLine("1. Amount                                   ");
            Console.WriteLine("2. Name                                     ");
            Console.WriteLine("3. Rate                                     ");
            Console.WriteLine("4. Date                                     ");
            Console.WriteLine("5. Category                                 ");
            Console.WriteLine("6. All                                      ");
        }

        internal static void DisplayWishlistOptions()
        {
            Console.Clear();
            Console.WriteLine("Select a wishlist option. \n\n");

            foreach (WishlistOption menuItem in Enum.GetValues<WishlistOption>())
            {
                var menuDescription = menuItem.GetDescription();
                var menuLine = string.Format("{0}. {1}", (int)menuItem, menuDescription);

                Console.WriteLine(menuLine);
            }
        }

        internal static void DisplayBudgetSummaryOptions()
        {
            Console.Clear();
            Console.WriteLine("Select a budget summary option. \n\n");

            foreach (BudgetSummaryOption menuItem in Enum.GetValues<BudgetSummaryOption>())
            {
                var menuDescription = menuItem.GetDescription();
                var menuLine = string.Format("{0}. {1}", (int)menuItem, menuDescription);

                Console.WriteLine(menuLine);
            }
        }

        internal static void DisplayIncomeOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an income option.  \n\n");

            foreach (IncomeOption menuItem in Enum.GetValues<IncomeOption>())
            {
                var menuDescription = menuItem.GetDescription();
                var menuLine = string.Format("{0}. {1}", (int)menuItem, menuDescription);

                Console.WriteLine(menuLine);
            }
        }

        internal static void DisplayRateOptions()
        {
            Console.Clear();
            Console.WriteLine("Select a rate option. \n\n");

            foreach (Rate menuItem in Enum.GetValues<Rate>())
            {
                var menuDescription = menuItem.GetDescription();
                var menuLine = string.Format("{0}. {1}", (int)menuItem, menuDescription);

                Console.WriteLine(menuLine);
            }
        }

        internal static void DisplayIncomeUpdateOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an income update option. \n\n");

            Console.WriteLine("1. Name                             ");
            Console.WriteLine("2. Amount                           ");
            Console.WriteLine("3. Rate                             ");
            Console.WriteLine("4. Logout                           ");
        }

        internal static void DisplayBudgetSummary()
        {
            //Add console table in to display budget summary
        }

        internal static void ViewActivity()
        {
            //Add console table in to display past expenses, incomes, and updates
        }
    }
}

