using System.ComponentModel;

namespace BudgetApp.UI
{
	public static class Validator
	{
        public static T? Convert<T>(string prompt)
		{
			bool valid = false;
			string userInput;

			while(!valid)
			{
				userInput = Utilities.GetUserInput(prompt);

                try
                {
					var converter = TypeDescriptor.GetConverter(typeof(T));

                   if (converter == null)
                    {
                        throw new Exception();
                    }
                    
                    var retVal = converter.ConvertFromString(userInput);

					if (retVal == null)
					{
						throw new Exception();
					}
					else
					{
						return (T)retVal;
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

