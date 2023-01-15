using System;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public class BudgetApp
	{
		static void Main(string[] args)
		{
			AppScreen.Welcome();
			string username = Validator.Convert<string>("your card number");
			Console.WriteLine($"Your card number is {username}");

			Utilities.PressEnterToContinue();
		}
	}
}

