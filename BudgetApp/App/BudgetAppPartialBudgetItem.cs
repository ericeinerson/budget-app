using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                string? startDateString = string.IsNullOrEmpty(item.StartDate.ToString()) ? "No Start Date" : item.StartDate?.ToString("MMMM dd, yyyy");
                string? endDateString = string.IsNullOrEmpty(item.EndDate.ToString()) ? "No End Date" : item.EndDate?.ToString("MMMM dd, yyyy");
                Category? category = selectedAccount.CategoryList.Find(t => t.Id == item.CategoryId);
                var categoryName = category != null ? category.Name : "No Category";

                allItemsTable.AddRow(item.Name, amountFormatted, startDateString, endDateString, item.Id, categoryName, type, item.Rate, item.AmountVariable);
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
            BudgetItem item = ConstructBudgetItem();

            if(item.Rate == Rate.NoRate)
            {
                AddSingleTransaction(item, type);
            }
            else
            {
                CreateMultipleTransactions(item, type);
            }

            var amountFormatted = Utilities.FormatAmount(item.Amount);
            var startDateString = string.Empty;
            var endDateString = string.Empty;
            var budgetItemList = GetBudgetItemList(type);

            if (!string.IsNullOrEmpty(item.StartDate.ToString()))
            {
                startDateString = string.Format($"This item will start on {item.StartDate:MMMM dd, yyyy}");
            }
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
                    AmountVariable = item.AmountVariable
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
                    AmountVariable = item.AmountVariable
                };
                selectedAccount.IncomeIdCounter++;
                budgetItemList.Add(incomeItem);

            }

            Utilities.PrintMessage($"You have succcessfully added " +
                $"{item.Name} with a value of " +
                $"{amountFormatted}. " +
                $"This transaction will start on {startDateString}{endDateString}" +
                $"!", true, false);
            ProcessBudgetItemOption(type);
        }

        public void RemoveBudgetItem(BudgetItemType type)
        {
            BudgetItem item = FindBudgetItem(type);
            if(item.Id == -1)
            {
                return;
            }
            var amountFormatted = Utilities.FormatAmount(item.Amount);
            var itemList = GetBudgetItemList(type);
            var transactionsConnected = selectedAccount.TransactionList.Where(x => x.BudgetItemId == item.Id);
            if (item != null)
            {
                itemList.Remove(item);
                Utilities.PrintMessage($"You have succcessfully removed {item.Name} with a value of {amountFormatted}!", true, false);
            }

            foreach(var transaction in transactionsConnected.ToList())
            {
                Utilities.PrintMessage($"The following transaction was remmoved:" +
                    $"Id: {transaction.Id}; " +
                    $"Category Id: {transaction.Id}; " +
                    $"Budget Item Id: {transaction.BudgetItemId}; " +
                    $"Name: {transaction.Name}; " +
                    $"Amount: {Utilities.FormatAmount(transaction.Amount)}; " +
                    $"Created Date {transaction.CreatedDate}; " +
                    $"Deleted Date: {DateTime.Now}; " +
                    $"Posted Date: {transaction.PostedDate:MMMM dd yyyy}; " +
                    $"Budget Item Type: {transaction.BudgetItemType} " +
                    $"Status: {Status.Cancelled}", true, false);
                selectedAccount.TransactionList.Remove(transaction);
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
                        var startDateString = string.IsNullOrEmpty(listItem.StartDate.ToString()) ? "No start date" : listItem.StartDate?.ToString("MMMM dd yyyy");
                        var endDateString = string.IsNullOrEmpty(listItem.EndDate.ToString()) ? "No end date" : listItem.EndDate?.ToString("MMMM dd yyyy");

                        var isCorrectItem = Utilities.PromptYesOrNo($"Is this the correct item: " +
                            $"Name: {listItem.Name}, " +
                            $"Amount: {Utilities.FormatAmount(listItem.Amount)}, " +
                            $"Id: {listItem.Id}, " +
                            $"Variable?: {listItem.AmountVariable}, " +
                            $"Category Id: {listItem.CategoryId}, " +
                            $"Start Date: {startDateString}, " +
                            $"End Date: {endDateString}, " +
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
                            var startDateString = string.IsNullOrEmpty(listItem.StartDate.ToString()) ? "No start date" : listItem.StartDate?.ToString("MMMM dd yyyy");
                            var endDateString = string.IsNullOrEmpty(listItem.EndDate.ToString()) ? "No end date" : listItem.EndDate?.ToString("MMMM dd yyyy");
                            string isCorrectItem = Utilities.PromptYesOrNo($"Is this the correct item: " +
                                $"Name: {listItem.Name}, " +
                                $"Amount: {Utilities.FormatAmount(listItem.Amount)}, " +
                                $"Id: {listItem.Id}, " +
                                $"Variable?: {listItem.AmountVariable}, " +
                                $"Category Id: {listItem.CategoryId}, " +
                                $"Start Date: {startDateString}, " +
                                $"End Date: {endDateString}, " +
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
                            var startDateString = string.IsNullOrEmpty(listItem.StartDate.ToString()) ? "No start date" : listItem.StartDate?.ToString("MMMM dd yyyy");
                            var endDateString = string.IsNullOrEmpty(listItem.EndDate.ToString()) ? "No end date" : listItem.EndDate?.ToString("MMMM dd yyyy");
                            string isCorrectItem = Utilities.PromptYesOrNo($"Is this the correct item: " +
                                $"Name: {listItem.Name}, " +
                                $"Amount: {Utilities.FormatAmount(listItem.Amount)}, " +
                                $"Id: {listItem.Id}, " +
                                $"Variable?: {listItem.AmountVariable}, " +
                                $"Category Id: {listItem.CategoryId}, " +
                                $"Start Date: {startDateString}, " +
                                $"End Date: {endDateString}, " +
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

            return new BudgetItem() { Id = -1 };
        }

        public BudgetItem ConstructBudgetItem()
        {
            var item = new BudgetItem();
            DateTime? startDate = null;
            DateTime? endDate = null;
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
            if (rate != Rate.NoRate)
            {
                Console.WriteLine("Please specify start date details");
                startDate = AssignDateForPastPresentOrFuture();
                Console.WriteLine("Please specify end date details");
                endDate = AssignDateForPastPresentOrFuture();
                while (startDate > endDate)
                {
                    Utilities.PrintMessage("Start date cannot be after end date. Please try again", false, true);
                    endDate = Utilities.ConstructDate();
                }
            }

            item.Id = id;
            item.CategoryId = category.Id;
            item.Name = name;
            item.Amount = amount;
            item.StartDate = startDate;
            item.EndDate = endDate;
            item.Rate = rate;
            item.AmountVariable = amountVariable;

            return item;
        }

        private static DateTime AssignDateForPastPresentOrFuture()
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
                            AssignDateForPastPresentOrFuture();
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
                    if (date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1))
                    {
                        Utilities.PrintMessage("Specified date is not in the future", false, false);
                        string goBack = Utilities.PromptYesOrNo("Go back?");
                        if (goBack == "y")
                        {
                            AssignDateForPastPresentOrFuture();
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
            if(item.Id == -1)
            {
                return;
            }
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
            if (item.StartDate != null || item.EndDate != null)
            {
                var updateStartDate = Utilities.PromptYesOrNo("Update start date?");
                var updateEndDate = Utilities.PromptYesOrNo("Update end date?");
                if (updateStartDate == "y")
                {
                    Console.WriteLine("Enter start date details");
                    item.StartDate = Utilities.ConstructDate();
                }
                if (updateEndDate == "y")
                {
                    Console.WriteLine("Enter end date details");
                    item.EndDate = Utilities.ConstructDate();
                }
                while (item.StartDate > item.EndDate)
                {
                    Utilities.PrintMessage("Start date cannot be after end date. Please try again", false, true);
                    Console.WriteLine("Enter start date details");
                    item.StartDate = Utilities.ConstructDate();
                    Console.WriteLine("Enter end date details");
                    item.EndDate = Utilities.ConstructDate();
                }
            }
            else
            {
                Utilities.PrintMessage("No start or end date exists.", false, false);
                var createDateRange = Utilities.PromptYesOrNo("Create a start/end date range?");
                if (createDateRange == "y")
                {
                    Console.WriteLine("Enter start date details");
                    item.StartDate = Utilities.ConstructDate();
                    Console.WriteLine("Enter end date details");
                    item.EndDate = Utilities.ConstructDate();
                    while (item.StartDate > item.EndDate)
                    {
                        Utilities.PrintMessage("Start date cannot be after end date. Please try again", false, true);
                        Console.WriteLine("Enter start date details");
                        item.StartDate = Utilities.ConstructDate();
                        Console.WriteLine("Enter end date details");
                        item.EndDate = Utilities.ConstructDate();
                    }
                }
            }
            VerifyTransactionsOutOfDateRange(item);
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

