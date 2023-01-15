using System;
namespace BudgetApp.UI
{
	public static class Utilities
	{

        public static void PrintMessage(string msg, bool success)
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
            PressEnterToContinue();
        }

        public static string GetUserInput(string prompt)
        {
            Console.WriteLine($"Enter {prompt}");
            return Console.ReadLine();
        }

        public static void PressEnterToContinue()
        {
            Console.WriteLine("\n\nPress enter to continue...\n");
            Console.ReadLine();
        }
    }
}

