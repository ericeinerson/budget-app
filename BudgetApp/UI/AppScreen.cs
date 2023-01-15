using System;
using BudgetApp.Domain.Entities;

namespace BudgetApp.UI
{
	public class AppScreen
	{
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

            tempUserAccount.FullName = Validator.Convert<string>("your passcode.").ToLower();
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

            Utilities.PressEnterToContinue();
            Environment.Exit(1);
        }
    }
}

