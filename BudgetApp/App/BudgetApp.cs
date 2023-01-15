using System;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public class BudgetApp
	{
		static void Main(string[] args)
		{
			AppScreen.Welcome();
			int passcode = Validator.Convert<int>("your passcode");
			Console.WriteLine($"Your passcode is {passcode}");

			Utilities.PressEnterToContinue();
		}
	}
}

