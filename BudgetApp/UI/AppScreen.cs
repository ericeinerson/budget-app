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
        AppMenu[] appMenuValues = Enum.GetValues<AppMenu>();
        ExpenseOption[] expenseOptionValues = Enum.GetValues<ExpenseOption>();
        IncomeOption[] incomeOptionValues = Enum.GetValues<IncomeOption>();
        CategoryOption[] categoryOptionValues = Enum.GetValues<CategoryOption>();
        WishlistOption[] wishlistOptionValues = Enum.GetValues<WishlistOption>();

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
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.FullName = Utilities.GetUserInput("your name.").ToLower();
            tempUserAccount.Passcode = Utilities.GetSecretInput("Enter your passcode.");

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

        internal static void DisplayExpenseSummary()
        {
            //Add console table in to display expenses
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

            Console.WriteLine("1. Weekly                     ");
            Console.WriteLine("2. Biweekly                   ");
            Console.WriteLine("3. Monthly                    ");
            Console.WriteLine("4. Yearly                     ");
            Console.WriteLine("5. No Rate                    ");
            Console.WriteLine("6. Other                      ");
            Console.WriteLine("7. Return to App Sceen        ");
            Console.WriteLine("8. Logout                     ");


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

        internal static void DisplayBudgetSummaryOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an option. \n\n                        ");

            Console.WriteLine("1. Budget Summary For Current Month And Year  ");
            Console.WriteLine("2. Budget Summary At Specified Future Date    ");
            Console.WriteLine("3. View Previous Transactions                 ");
            Console.WriteLine("4. Logout                                     ");
            Console.WriteLine("5. App Menu                                   ");
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

