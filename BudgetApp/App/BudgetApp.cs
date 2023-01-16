using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Entities.Interfaces;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public class BudgetApp : IUserLogin, IUserAccountActions, IUpdate
	{
		private List<UserAccount> userAccountList;
		private UserAccount selectedAccount;

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserPasscode();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            AppScreen.DisplayAppMenu();
            ProcessMenuOption();
        }

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
        private void ProcessMenuOption()
        {
            switch(Validator.Convert<int>("an option."))
            {
                case (int)AppMenu.BudgetSummary:
                    BudgetSummary();
                    break;
                case (int)AppMenu.PreviousMonths:
                    Console.WriteLine("Checking previous months");
                    break;
                case (int)AppMenu.Incomes:
                    Console.WriteLine("Managing incomes");
                    break;
                case (int)AppMenu.CategorizedExpenses:
                    Console.WriteLine("Managing expenses ");
                    break;
                case (int)AppMenu.Wishlist:
                    Console.WriteLine("Checking budget summary");
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.",true);
                    Run();
                    break;
                default:
                    Utilities.PrintMessage("Invalid option.",false);
                    break;
            }
        }

        public void BudgetSummary()
        {
            Utilities.PrintMessage($"Your future balance is {Utilities.FormatAmount(selectedAccount.Balance)}", true);
        }

        public void Incomes()
        {
            throw new NotImplementedException();
        }

        public void CategorizedExpenses()
        {
            var expense_amt = Validator.Convert<decimal>("expense amount");

            Console.WriteLine("\nProcessing expense");
            Utilities.PrintDotAnimation();
            Console.WriteLine("");

            if(expense_amt <= 0)
            {
                Utilities.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }

            if(PreviewUpdate(expense_amt) == false)
            {
                Utilities.PrintMessage("You have cancelled your action", false);
                return;
            }
        }

        private bool PreviewUpdate(decimal amount)
        {
            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{Utilities.FormatAmount(amount)}\n\n");
            Console.WriteLine("");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }

        public void InsertUpdate(UpdateType _updateType, decimal _updateAmount, string _description)
        {
            var update = new BudgetUpdate()
            {
                UpdateId = Utilities.GetUpdateId(),
                UpdateAmount = _updateAmount,
                UpdateType = _updateType,
                Description = _description,
                UpdateDate = DateTime.Now.Date.ToString("dd/MM/yyyy")
            };
        }

        public void ViewUpdate()
        {
            throw new NotImplementedException();
        }
    }
}

