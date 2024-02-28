using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;

namespace BudgetApp.App
{
    public partial class BudgetApp
    {
        private void ProcessBudgetItemMenuOption(BudgetItemType type)
        {
            switch (Validator.Convert<int>("an option"))
            {
                case (int)BudgetItemOption.ViewBudgetItem:
                    ViewBudgetItems(type);
                    break;
                case (int)BudgetItemOption.AddBudgetItem:
                    AddBudgetItem(type);
                    break;
                case (int)BudgetItemOption.RemoveBudgetItem:
                    RemoveBudgetItem(type);
                    break;
                case (int)BudgetItemOption.UpdateBudgetItemDetails:
                    UpdateBudgetItemDetails(type);
                    break;
                case (int)BudgetItemOption.Logout:
                    LogoutProgress();
                    break;
                case (int)BudgetItemOption.GoBack:
                    GoBackToAppScreen();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessBudgetItemMenuOption(type);
                    break;
            }
        }

        public void ViewBudgetItems(BudgetItemType type)
        {
            ConsoleTable allItemsTable = new("Name", "Amount","Start Date", "End Date","Id", "Category", "Budget Item Type");

            foreach (BudgetItem item in GetBudgetItemList(type))
            {
                string amountFormatted = Utilities.FormatAmount(item.Amount);
                string? endDate = string.IsNullOrEmpty(item.EndDate.ToString()) ? "No End Date" : item.EndDate?.ToString("MMMM dd, yyyy");
                Category? category = selectedAccount.CategoryList.Find(t => t.Id == item.CategoryId);
                var categoryName = category != null ? category.Name : string.Empty;

                allItemsTable.AddRow(item.Name, amountFormatted, item.StartDate.ToString("MMMM dd, yyyy"), endDate, item.Id, categoryName, type);
            }
            allItemsTable.Write();
            Utilities.PressEnterToContinue();
        }

        public List<BudgetItem> GetBudgetItemList(BudgetItemType type)
        {
            List<BudgetItem> budgetItemList;

            if (type == BudgetItemType.Expense)
            {
                budgetItemList = selectedAccount.ExpenseList;
            }
            else if (type == BudgetItemType.Income)
            {
                budgetItemList = selectedAccount.IncomeList;
            }
            else
            {
                throw new Exception();
            }

            return budgetItemList;
        }

        public void AddBudgetItem(BudgetItemType type)
        {
            BudgetItem item = ConstructBudgetItem(type);

            if(item.Rate == Rate.NoRate)
            {
                AddSingleTransaction(item, type);
            }
            else
            {
                CreateMultipleTransactions(item, type);
            }

            var amountFormatted = Utilities.FormatAmount(item.Amount);
            string endDateString = string.Empty;
            var budgetItemList = GetBudgetItemList(type);

            if (!string.IsNullOrEmpty(item.EndDate.ToString()))
            {
                endDateString = string.Format($" and will end on {item.EndDate:MMMM dd, yyyy}");
            }

            if(type == BudgetItemType.Expense)
            {
                var expenseItem = new Expense()
                {
                    Id = item.Id,
                    CategoryId = selectedAccount.ExpenseIdCounter,
                    Name = item.Name,
                    Amount = item.Amount,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Rate = item.Rate,
                };
                selectedAccount.ExpenseIdCounter++;

                budgetItemList.Add(expenseItem);
            }
            else if(type == BudgetItemType.Income)
            {
                var incomeItem = new Income()
                {
                    Id = item.Id,
                    CategoryId = selectedAccount.IncomeIdCounter,
                    Name = item.Name,
                    Amount = item.Amount,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Rate = item.Rate,
                };
                selectedAccount.IncomeIdCounter++;
                budgetItemList.Add(incomeItem);

            }

            Utilities.PrintMessage($"You have succcessfully added {item.Name} with a value of {amountFormatted}. This transaction will start on {item.StartDate:MMMM dd, yyyy}{endDateString}!", true, false);
        }

        public void RemoveBudgetItem(BudgetItemType type)
        {
            BudgetItem item = FindBudgetItem(type);

            var amountFormatted = Utilities.FormatAmount(item.Amount);
            var itemList = GetBudgetItemList(type);

            if (item != null)
            {
                itemList.Remove(item);
                Utilities.PrintMessage($"You have succcessfully removed {item.Name} with a value of {amountFormatted}!", true, false);
            }
        }

        public BudgetItem FindBudgetItem(BudgetItemType type = BudgetItemType.None)
        {
            BudgetItem? item = null;
            if(type == BudgetItemType.None)
            {
                string typeSelection = Utilities.GetUserInput("1 for expense, 2 for income");
                while(typeSelection != "1" && typeSelection != "2")
                { 
                    Utilities.PrintMessage("Invalid input. Try again", false, false);
                    typeSelection = Utilities.GetUserInput("1 for expense, 2 for income");
                }
                if(typeSelection == "1")
                {
                    type = BudgetItemType.Expense;
                }
                else if(typeSelection == "2")
                {
                    type = BudgetItemType.Income;
                }
            }
            var transactionList = GetBudgetItemList(type);
            while(item == null)
            {
                string itemName = Utilities.GetUserInput("name. If not known, enter n to skip or a to exit to app menu").ToLower();
                if(itemName == "a")
                {
                    break;
                }
                item = transactionList.FirstOrDefault(t => t.Name.ToLower() == itemName.ToLower());
                if (itemName == "n")
                {
                    decimal amount = Validator.Convert<decimal>("amount");
                    item = transactionList.FirstOrDefault(t => t.Amount == amount);
                }

                if(item == null)
                {
                    Utilities.PrintMessage("Sorry, transaction not found. Please try again", false, false);
                }
            }

            if(item == null)
            {
                throw new NullReferenceException();
            }

            return item;
        }

        public BudgetItem ConstructBudgetItem(BudgetItemType type)
        {
            var item = new BudgetItem();
            var categoryId = 0;
            var amountVariable = false;
            int id;

            if (type == BudgetItemType.Expense)
            {
                selectedAccount.ExpenseIdCounter++;
                id = selectedAccount.ExpenseIdCounter;
            }
            else if (type == BudgetItemType.Income)
            {
                selectedAccount.IncomeIdCounter++;
                id = selectedAccount.IncomeIdCounter;
            }
            else
            {
                throw new Exception();
            }
            string name = Utilities.GetUserInput("name");
            decimal amount = Validator.Convert<decimal>("amount");
            string amountVariablePrompt = Utilities.PromptYesOrNo("Is this a variable amount?");
            if(amountVariablePrompt == "y")
            {
                amountVariable = true;
            }

            Category category = AssignTransactionCategory();

            //TEST CODE REMOVE LATER START
            if (category != null)
            {
                categoryId = category.Id;
                var categoryName = category.Name;

                Console.WriteLine(categoryId);
                Console.WriteLine(categoryName);
            }
            else
            {
                Console.WriteLine("Successfully exited the while loop with q");
            }
            Utilities.PressEnterToContinue();
            //TEST CODE REMOVE LATER END

            Rate rate = ProcessRateOption();
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string todayOrFuture = Utilities.GetUserInput("whether t for today or f for future").ToLower();

            if (todayOrFuture == "t")
            {
                startDate = DateTime.Now;
            }
            else if (todayOrFuture == "f")
            {
                Console.WriteLine("Please specify start date details");
                startDate = Utilities.ConstructDate();
            }

            Console.WriteLine("Please specify end date details");
            endDate = Utilities.ConstructDate();

            if(startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            item.Id = id;
            item.Name = name;
            item.Amount = amount;
            item.Rate = rate;
            item.StartDate = startDate;
            item.EndDate = endDate;
            item.CategoryId = category == null ? 0 : category.Id;
            item.AmountVariable = amountVariable;

            return item;
        }

        private void UpdateBudgetItemDetails(BudgetItemType type)
        {
            var item = FindBudgetItem(type);

            AppScreen.DisplayBudgetItemUpdateDetails();

            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    UpdateBudgetItemAmount(item);
                    break;
                case 2:
                    UpdateBudgetItemName(item);
                    break;
                case 3:
                    UpdateBudgetItemRate(item);
                    break;
                case 4:
                    UpdateBudgetItemDate(item);
                    break;
                case 5:
                    UpdateBudgetItemCategory(item);
                    break;
                case 6:
                    UpdateAllBudgetItemDetails(item);
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    UpdateBudgetItemDetails(type);
                    break;
            }
        }

        private void UpdateBudgetItemAmount(BudgetItem item)
        {
            var amount = Validator.Convert<decimal>("new amount");
            item.Amount = amount;
        }

        private void UpdateBudgetItemName(BudgetItem item)
        {
            var name = Utilities.GetUserInput("new name");
            item.Name = name;
        }

        private void UpdateBudgetItemRate(BudgetItem item)
        {
            var rate = ProcessRateOption();
            item.Rate = rate;
        }

        private void UpdateBudgetItemDate(BudgetItem item)
        {
            var date = Utilities.ConstructDate();
            item.StartDate = date;
        }

        private void UpdateBudgetItemCategory(BudgetItem item)
        {
            var categoryId = AssignTransactionCategory().Id;
            item.CategoryId = categoryId;
        }

        private void UpdateAllBudgetItemDetails(BudgetItem item)
        {
            UpdateBudgetItemAmount(item);
            UpdateBudgetItemName(item);
            UpdateBudgetItemRate(item);
            UpdateBudgetItemDate(item);
            UpdateBudgetItemCategory(item);
        }
    }
}

