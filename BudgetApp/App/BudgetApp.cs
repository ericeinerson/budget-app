using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Entities.Interfaces;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public class BudgetApp : IUserLogin
	{
		private List<UserAccount> userAccountList;
		private UserAccount selectedAccount;

        public void InitializeData()
		{
			userAccountList = new List<UserAccount>
			{
				new UserAccount
				{
					Id = 1,
					FullName = "Eric Einerson",
					Passcode = 0991,
					Balance = 0,
					IsLocked = false,
					TotalLogin = 0
				},
                new UserAccount
                {
                    Id = 1,
                    FullName = "Pickle Rick",
                    Passcode = 8374,
                    Balance = 0,
                    IsLocked = true,
                    TotalLogin = 0
                },
                new UserAccount
                {
                    Id = 1,
                    FullName = "Random User",
                    Passcode = 1111,
                    Balance = 0,
                    IsLocked = false,
                    TotalLogin = 0
                }
            };
		}
        public void CheckUserPasscode()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if (inputAccount.FullName.ToLower().Equals(selectedAccount.FullName.ToLower()))
                    {
                        selectedAccount.TotalLogin++;

                        if (inputAccount.Passcode.Equals(selectedAccount.Passcode))
                        {
                            selectedAccount = account;

                            if(selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                    if (isCorrectLogin == false)
                    {
                        Utilities.PrintMessage("\nInvalid name or passcode", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                        if (selectedAccount.IsLocked)
                        {
                            AppScreen.PrintLockScreen();
                        }
                    }
                    Console.Clear();
                }
            }
        }
        public void Welcome()
        {
            Console.WriteLine($"Welcome back, {selectedAccount.FullName}");
        }
    }
}

