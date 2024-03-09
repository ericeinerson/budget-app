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
            ConsoleTable allItemsTable = new("Name", "Amount","Start Date", "End Date","Id", "Category", "Budget Item Type", "Rate", "Variable?");

            foreach (BudgetItem item in GetBudgetItemList(type))
            {
                string amountFormatted = Utilities.FormatAmount(item.Amount);
                string? endDate = string.IsNullOrEmpty(item.EndDate.ToString()) ? "No End Date" : item.EndDate?.ToString("MMMM dd, yyyy");
                Category? category = selectedAccount.CategoryList.Find(t => t.Id == item.CategoryId);
                var categoryName = category != null ? category.Name : "No Category";

                allItemsTable.AddRow(item.Name, amountFormatted, item.StartDate.ToString("MMMM dd, yyyy"), endDate, item.Id, categoryName, type, item.Rate, item.AmountVariable);
            }
            allItemsTable.Write();
            Utilities.PressEnterToContinue();
            ProcessBudgetItemOption(type);
        }

        public List<BudgetItem> GetBudgetItemList(BudgetItemType type)
        {
            List<BudgetItem> budgetItemList = new();

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
                Utilities.PrintMessage("Budget item type is not valid", false, false);
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
                    CategoryId = item.CategoryId,
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
                    CategoryId = item.CategoryId,
                    Name = item.Name,
                    Amount = item.Amount,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Rate = item.Rate,
                };
                selectedAccount.IncomeIdCounter++;
                budgetItemList.Add(incomeItem);

            }

            Utilities.PrintMessage($"You have succcessfully added " +
                $"{item.Name} with a value of " +
                $"{amountFormatted}. " +
                $"This transaction will start on {item.StartDate:MMMM dd, yyyy}{endDateString}" +
                $"!", true, false);
            ProcessBudgetItemOption(type);
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
            ProcessBudgetItemOption(type);
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
                string itemName = Utilities.GetUserInput("name. If not known, enter a to find by amount, i to find by id, or q to exit to app menu").ToLower();
                var itemList = new List<BudgetItem>();
                if(itemName == "q")
                {
                    break;
                }
                itemList = transactionList.Where(t => t.Name.ToLower() == itemName.ToLower()).ToList();
                if(itemList.Any())
                {
                    foreach(var listItem in itemList)
                    {
                        string isCorrectItem = Utilities.PromptYesOrNo($"Is this the correct item: " +
                            $"Name: {listItem.Name}, " +
                            $"Amount: {listItem.Amount}, " +
                            $"Id: {listItem.Id}, " +
                            $"Variable?: {listItem.AmountVariable}, " +
                            $"Category Id: {listItem.CategoryId}, " +
                            $"Start Date: {listItem.StartDate}, " +
                            $"End Date: {listItem.EndDate}, " +
                            $"Rate: {listItem.Rate}");

                        if(isCorrectItem == "y")
                        {
                            item = listItem;
                            break;
                        }
                    }
                }
                if (itemName == "a")
                {
                    decimal amount = Validator.Convert<decimal>("amount");
                    itemList = transactionList.Where(t => t.Amount == amount).ToList();
                    if (itemList.Count > 0)
                    {
                        foreach (var listItem in itemList)
                        {
                            string isCorrectItem = Utilities.PromptYesOrNo($"Is this the correct item: " +
                                $"Name: {listItem.Name}, " +
                                $"Amount: {listItem.Amount}, " +
                                $"Id: {listItem.Id}, " +
                                $"Variable?: {listItem.AmountVariable}, " +
                                $"Category Id: {listItem.CategoryId}, " +
                                $"Start Date: {listItem.StartDate}, " +
                                $"End Date: {listItem.EndDate}, " +
                                $"Rate: {listItem.Rate}");

                            if (isCorrectItem == "y")
                            {
                                item = listItem;
                                break;
                            }
                        }
                    }
                }
                if (itemName == "i")
                {
                    decimal id = Validator.Convert<decimal>("id");
                    itemList = transactionList.Where(t => t.Id == id).ToList();
                    if (itemList.Count > 0)
                    {
                        foreach (var listItem in itemList)
                        {
                            string isCorrectItem = Utilities.PromptYesOrNo($"Is this the correct item: " +
                                $"Name: {listItem.Name}, " +
                                $"Amount: {listItem.Amount}, " +
                                $"Id: {listItem.Id}, " +
                                $"Variable?: {listItem.AmountVariable}, " +
                                $"Category Id: {listItem.CategoryId}, " +
                                $"Start Date: {listItem.StartDate}, " +
                                $"End Date: {listItem.EndDate}, " +
                                $"Rate: {listItem.Rate}");

                            if (isCorrectItem == "y")
                            {
                                item = listItem;
                                break;
                            }
                        }
                    }
                }
                if (item == null)
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
            var amountVariable = false;
            int id = AssignBudgetItemId();

            string name = Utilities.GetUserInput("name");
            decimal amount = Validator.Convert<decimal>("amount");
            string amountVariablePrompt = Utilities.PromptYesOrNo("Is this a variable amount?");
            if (amountVariablePrompt == "y")
            {
                amountVariable = true;
            }

            Category category = AssignTransactionCategory();

            Rate rate = ProcessRateOption();
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            startDate = AssignDate();

            Console.WriteLine("Please specify end date details");
            endDate = Utilities.ConstructDate();

            while (startDate > endDate)
            {
                Utilities.PrintMessage("Start date cannot be after end date. Please try again", false, true);
                endDate = Utilities.ConstructDate();
            }

            item.Id = id;
            item.CategoryId = category == null ? 0 : category.Id;
            item.Name = name;
            item.Amount = amount;
            item.StartDate = startDate;
            item.EndDate = endDate;
            item.Rate = rate;
            item.AmountVariable = amountVariable;

            return item;
        }

        private static DateTime AssignDate()
        {
            DateTime date = new();
            string pastPresentOrFuture = Utilities.GetUserInput("t for today, p for a past day, or f for a different day").ToLower();
            while (pastPresentOrFuture != "t" && pastPresentOrFuture != "p" && pastPresentOrFuture != "f")
            {
                Utilities.PrintMessage("Invalid entry. Please try again", false, false);
                pastPresentOrFuture = Utilities.GetUserInput("t for today, p for a past day, or f for a different day").ToLower();
            }
            if (pastPresentOrFuture == "t")
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            else if (pastPresentOrFuture == "p" || pastPresentOrFuture == "f")
            {
                Console.WriteLine("Please specify start date details");
                date = Utilities.ConstructDate();
                while (pastPresentOrFuture == "p")
                {
                    if (date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    {
                        Utilities.PrintMessage("Specified date is not in the past", false, false);
                        string goBack = Utilities.PromptYesOrNo("Go back?");
                        if (goBack == "y")
                        {
                            AssignDate();
                        }
                        else
                        {
                            date = Utilities.ConstructDate();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                while (pastPresentOrFuture == "f")
                {
                    if (date <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1))
                    {
                        Utilities.PrintMessage("Specified date is not in the future", false, false);
                        string goBack = Utilities.PromptYesOrNo("Go back?");
                        if (goBack == "y")
                        {
                            AssignDate();
                        }
                        else
                        {
                            date = Utilities.ConstructDate();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return date;
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
            ProcessBudgetItemOption(type);
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

        private int AssignBudgetItemId()
        {
            selectedAccount.BudgetItemIdCounter++;
            return selectedAccount.BudgetItemIdCounter;
        }
    }
}

