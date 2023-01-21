using System;
using BudgetApp.Domain.Entities;
using BudgetApp.App;

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
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.FullName = Validator.Convert<string>("your name.").ToLower();
            tempUserAccount.Passcode = Convert.ToInt32(Utilities.GetSecretInput("Enter your passcode."));

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
            Console.WriteLine("1. Budget Summary               ");
            Console.WriteLine("2. Instructions/App Description ");
            Console.WriteLine("3. Incomes                      ");
            Console.WriteLine("4. Categorized Expenses         ");
            Console.WriteLine("5. Wishlist/Future              ");
            Console.WriteLine("6. Logout                       ");
            Console.WriteLine("7. Update Balance               ");

        }

        internal static void DisplayExpenseOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an expense to update. \n\n");

            Console.WriteLine("1. Rent/Utilities           6. Medical      ");
            Console.WriteLine("2. Credit Cards             7. Insurance    ");
            Console.WriteLine("3. Food/General             8. Subscriptions");
            Console.WriteLine("4. Loans                    9. Gym          ");
            Console.WriteLine("5. Gas                      10. Other       ");
            Console.WriteLine("11. Logout                  12. App Menu    ");

        }

        internal static void DisplayExpenseUpdateOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an update option. \n\n");

            Console.WriteLine("1. Pay All/Remaining                        ");
            Console.WriteLine("2. Pay Partial                              ");
            Console.WriteLine("3. Enter new amount                         ");
            Console.WriteLine("4. Logout                                   ");

        }

        internal static void DisplayIncomeOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an income to update. \n\n");

            Console.WriteLine("1. Paychecks             ");
            Console.WriteLine("2. Taxes                 ");
            Console.WriteLine("3. Other                 ");
            Console.WriteLine("4. Return To Main Menu   ");
            Console.WriteLine("5. Exit App              ");
        }

        internal static void DisplayWishlistOptions()
        {
            Console.Clear();
            Console.WriteLine("Select a wishlist option. \n\n");

            Console.WriteLine("1. View Wishlist               ");
            Console.WriteLine("2. Add Wishlist Item           ");
            Console.WriteLine("3. Make Wishlist Expense       ");
            Console.WriteLine("4. Logout                      ");

        }

        internal static void DisplayExpenseSummary()
        {
            //Add console table in to display expenses
        }

        internal static void DisplayIncomeMenu()
        {
            Console.Clear();
            Console.WriteLine("Select an income option. \n\n");

            Console.WriteLine("1. View Incomes               ");
            Console.WriteLine("2. Add an Income              ");
            Console.WriteLine("3. Update an Income           ");
            Console.WriteLine("4. Logout                     ");

        }

        internal static void DisplayRateOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an income rate option. \n\n");

            Console.WriteLine("1. Weekly                     ");
            Console.WriteLine("2. Biweekly                   ");
            Console.WriteLine("3. Monthly                    ");
            Console.WriteLine("4. Yearly                     ");
            Console.WriteLine("5. Logout                     ");

        }

        internal static void DisplayIncomeUpdateOptions()
        {
            Console.Clear();
            Console.WriteLine("Select an income update option. \n\n");

            Console.WriteLine("1. Name                     ");
            Console.WriteLine("2. Amount                   ");
            Console.WriteLine("3. Rate                    ");
            Console.WriteLine("4. Logout                     ");

        }

        internal static void DisplayBudgetSummary()
        {
            //Add console table in to display budget summary
        }

        internal static void ViewActivity()
        {
            //Add console table in to display past expenses, incomes, and updates
        }

        internal static void LogoutProgress()
        {
            Console.WriteLine("Thank you for using My Budget App.");
            Utilities.PrintDotAnimation();
            Console.Clear();
            Console.WriteLine("Would you like to exit the app (Y/N)?");
            string response = string.Empty;

            while (response != "Y" || response != "N") {

                response = Console.ReadLine()!;

                if (response == "Y")
                {
                    Environment.Exit(1);
                }
                else if (response == "N")
                {
                    break;

                }
                Utilities.PrintMessage("Invalid entry. Try again", false, true);
                continue;
            }
        }
    }
}

