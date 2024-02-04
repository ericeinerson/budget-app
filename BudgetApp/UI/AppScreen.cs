using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Extensions;

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

        internal static void DisplayTransactionOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an option. \n\n");

            foreach (BudgetItemType menuItem in Enum.GetValues<BudgetItemType>())
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

        internal static void DisplayBudgetItemUpdateDetails()
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

        internal static void DisplayWishlistUpdateDetails()
        {
            Console.Clear();
            Console.WriteLine("Select a detail to update. \n\n");

            Console.WriteLine("1. Item                                     ");
            Console.WriteLine("2. Cost                                     ");
            Console.WriteLine("3. Priority                                 ");
            Console.WriteLine("4. All                                      ");
        }

        internal static void DisplayItemUpdateDetails()
        {
            Console.Clear();
            Console.WriteLine("Select an item update option.   \n\n");

            Console.WriteLine("1. Name                             ");
            Console.WriteLine("2. Cost                             ");
            Console.WriteLine("3. Priority                         ");
            Console.WriteLine("4. All                              ");
        }

        internal static void DisplayCategoryUpdateDetails()
        {
            Console.Clear();
            Console.WriteLine("Select a detail to update. \n\n");

            Console.WriteLine("1. Name                                     ");
            
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

        internal static void DisplayPostingOptions()
        {
            Console.Clear();
            Console.WriteLine("Select a posting option. \n\n");

            Console.WriteLine("1. All                              ");
            Console.WriteLine("2. Some                             ");
            Console.WriteLine("3. None                             ");
        }

        internal static void ViewActivity()
        {
            //Add console table in to display past expenses, incomes, and updates
        }
    }
}

