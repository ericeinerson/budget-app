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
        private Dictionary<ExpenseType, decimal> _expenses = new Dictionary<ExpenseType, decimal>
        {
            { ExpenseType.RentAndUtilities,14 },
            { ExpenseType.FoodAndGeneral,15 },
            { ExpenseType.CreditCards,0 },
            { ExpenseType.Loans,0 },
            { ExpenseType.Insurance,0 },
            { ExpenseType.Gas,0 },
            { ExpenseType.Medical,0 },
            { ExpenseType.Subscriptions,0 },
            { ExpenseType.Gym,0 },
            { ExpenseType.Other,16 }

        };

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
                    TotalIncomes = 0
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
                    TotalIncomes = 0
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
                    TotalIncomes = 0
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

        private ExpenseType ProcessExpenseMenuOption()
        {
            switch(Validator.Convert<int>("an option"))
            {
                case 1:
                    return ExpenseType.RentAndUtilities;
                    break;
                case 2:
                    return ExpenseType.CreditCards;
                    break;
                case 3:
                    return ExpenseType.FoodAndGeneral;
                    break;
                case 4:
                    return ExpenseType.Loans;
                    break;
                case 5:
                    return ExpenseType.Gas;
                    break;
                case 6:
                    return ExpenseType.Medical;
                    break;
                case 7:
                    return ExpenseType.Insurance;
                    break;
                case 8:
                    return ExpenseType.Subscriptions;
                    break;
                case 9:
                    return ExpenseType.Gym;
                    break;
                case 10:
                    return ExpenseType.Other;
                    break;
                case 11:
                    AppScreen.LogoutProgress();
                    Utilities.PrintMessage("You have successfully logged out.", true);
                    Run();
                    return ExpenseType.Undefined;
                    break;
                case 12:
                    AppScreen.DisplayAppMenu();
                    ProcessAppMenuOption();
                    return ExpenseType.Undefined;
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again",false);
                    return ProcessExpenseMenuOption();
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

            foreach(KeyValuePair<ExpenseType,decimal> expense in _expenses)
            {
                _sumOfAllExpenses += expense.Value;
            }
            Console.WriteLine($"\nSum of expenses: {_sumOfAllExpenses}");

            Utilities.PressEnterToContinue();
            AppScreen.DisplayExpenseOptions();
            ExpenseType expense_type = ExpenseType.Other;

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

