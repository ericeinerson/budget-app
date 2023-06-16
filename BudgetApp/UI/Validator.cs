using System;
using System.ComponentModel;

namespace BudgetApp.UI
{
	public static class Validator
	{
        public static T Convert<T>(string prompt)
		{
			bool valid = false;
			string userInput;

			while(!valid)
			{
				userInput = Utilities.GetUserInput(prompt);

				try
				{
					var converter = TypeDescriptor.GetConverter(typeof(T));
					if(converter == null)
					{
						return default;
					}
					else
					{
                        return (T)converter.ConvertFromString(userInput);
                    }
				}
				catch
				{
					Utilities.PrintMessage("Invalid input. Try again", false);
				}
			}
			return default;
		}
	}
}

