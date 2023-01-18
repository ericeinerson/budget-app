using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Entities.Interfaces;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public class BudgetApp : IUserLogin, IUserAccountActions, IUpdate
	{
		private List<UserAccount>? userAccountList;
		private UserAccount? selectedAccount;
        private List<BudgetUpdate>? _listOfUpdates;

        private decimal _amountNeeded;
        private decimal _currentBalance;
        private decimal _estimatedIncome = 9000.00M;
        private decimal _difference;
        private decimal _sumOfAllExpenses = 0;

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserPasscode();
            AppScreen.WelcomeCustomer(selectedAccount!.FullName!);
            AppScreen.DisplayAppMenu();
            ProcessAppMenuOption();
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
					TotalLogin = 0,
                    TotalExpenses = 0,
                    TotalIncomes = 0,
                    ExpenseCategories = new Dictionary<string, decimal>
                    {
                        { "other", 0 }
                    }
				},
                new UserAccount
                {
                    Id = 1,
                    FullName = "Pickle Rick",
                    Passcode = 8374,
                    Balance = 0,
                    IsLocked = true,
                    TotalLogin = 0,
                    TotalExpenses = 0,
                    TotalIncomes = 0,
                    ExpenseCategories = new Dictionary<string, decimal>
                    {
                        { "other", 0 }
                    }
                },
                new UserAccount
                {
                    Id = 1,
                    FullName = "Random User",
                    Passcode = 1111,
                    Balance = 0,
                    IsLocked = false,
                    TotalLogin = 0,
                    TotalExpenses = 0,
                    TotalIncomes = 0,
                    ExpenseCategories = new Dictionary<string, decimal>
                    {
                        { "other", 0 }
                    }
                }
            };

            _listOfUpdates = new List<BudgetUpdate>();
		}
        public void CheckUserPasscode()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach(UserAccount account in userAccountList!)
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
        private void ProcessAppMenuOption()
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
                    CategorizedExpenses();
                    break;
                case (int)AppMenu.Wishlist:
                    Console.WriteLine("Checking budget summary");
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.",true);
                    Run();
                    break;
                case (int)AppMenu.UpdateBalance:
                    UpdateBalance();
                    break;
                default:
                    Utilities.PrintMessage("Invalid option.",false);
                    break;
            }
            AppScreen.DisplayAppMenu();
            ProcessAppMenuOption();
        }

        private void ProcessExpenseMenuOption()
        {
            decimal monthlyExpenseAmount = 0;

            switch(Validator.Convert<int>("an expense to add"))
            {
                case 1:
                    Console.WriteLine("Please enter monthly expense amount");

                    monthlyExpenseAmount = Validator.Convert<decimal>("expense amount");

                    if (!selectedAccount.ExpenseCategories.ContainsKey("rent_utilities"))
                    {
                        selectedAccount.ExpenseCategories.Add("rent_utilities", monthlyExpenseAmount);
                        Utilities.PrintMessage($"Your monthly budget for rent and utilities is {Utilities.FormatAmount(selectedAccount.ExpenseCategories["rent_utilities"])}",true);
                    }
                    break;
                case 2:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("credit_cards"))
                    {
                        selectedAccount.ExpenseCategories.Add("credit_cards", monthlyExpenseAmount);
                    }
                    break;
                case 3:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("food_general"))
                    {
                        selectedAccount.ExpenseCategories.Add("food_general", monthlyExpenseAmount);
                    }
                    break;
                case 4:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("loans"))
                    {
                        selectedAccount.ExpenseCategories.Add("loans", monthlyExpenseAmount);
                    }
                    break;
                case 5:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("gas"))
                    {
                        selectedAccount.ExpenseCategories.Add("gas", monthlyExpenseAmount);
                    }
                    break;
                case 6:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("medical"))
                    {
                        selectedAccount.ExpenseCategories.Add("medical", monthlyExpenseAmount);
                    }
                    break;
                case 7:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("insurance"))
                    {
                        selectedAccount.ExpenseCategories.Add("insurance", monthlyExpenseAmount);
                    }
                    break;
                case 8:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("subscriptions"))
                    {
                        selectedAccount.ExpenseCategories.Add("subscriptions", monthlyExpenseAmount);
                    }
                    break;
                case 9:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("gym"))
                    {
                        selectedAccount.ExpenseCategories.Add("gym", monthlyExpenseAmount);
                    }
                    break;
                case 10:
                    if (!selectedAccount.ExpenseCategories.ContainsKey("other"))
                    {
                        selectedAccount.ExpenseCategories.Add("other", monthlyExpenseAmount);
                    }
                    break;
                case 11:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    break;
                case 12:
                    AppScreen.DisplayAppMenu();
                    ProcessAppMenuOption();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again",false);
                    ProcessExpenseMenuOption();
                    break;
            }
        }

        private void ProcessExpenseUpdateOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    Console.WriteLine("Pay All/Remaining");
                    break;
                case 2:
                    Console.WriteLine("Pay Partial");
                    break;
                case 3:
                    Console.WriteLine("Enter new amount");
                    break;
                case 4:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessExpenseUpdateOption();
                    break;
            }
        }

        public void UpdateBalance()
        {
            Console.WriteLine("Please enter your balance");
            _currentBalance = Validator.Convert<decimal>("current balance");
            Console.WriteLine($"\nYour current balance is {Utilities.FormatAmount(_currentBalance)}");

        }

        public void BudgetSummary()
        {
            _amountNeeded = _sumOfAllExpenses;
            _difference = _currentBalance + _estimatedIncome - _amountNeeded;
            Console.WriteLine(_amountNeeded);

            Utilities.PrintMessage($"Your future balance is {Utilities.FormatAmount(_difference)}", true);
        }

        public void Incomes()
        {
            throw new NotImplementedException();
        }

        public void CategorizedExpenses()
        {
            _sumOfAllExpenses = 0;

            foreach(KeyValuePair<string,decimal> expense in selectedAccount.ExpenseCategories)
            {
                _sumOfAllExpenses += expense.Value;
            }
            Console.WriteLine($"\nSum of expenses: {_sumOfAllExpenses}");

            Utilities.PressEnterToContinue();
            AppScreen.DisplayExpenseOptions();

            ProcessExpenseMenuOption();

            AppScreen.DisplayExpenseUpdateOptions();
            ProcessExpenseUpdateOption();
            Utilities.PressEnterToContinue();
            var expense_amt = Validator.Convert<decimal>("expense amount");

            //put in calculation here

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

            InsertUpdate(UpdateType.Expense, expense_amt, "");
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

            _listOfUpdates!.Add(update);
        }

        public void ViewUpdate()
        {
            throw new NotImplementedException();
        }
    }
}

