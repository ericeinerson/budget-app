using System;
using System.Net.NetworkInformation;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BudgetApp.App
{
	public partial class BudgetApp
    {
        public void VerifyTransactionStatus()
        {
            var transactionsFlagged = selectedAccount.TransactionList.Where(t => (t.Status == Status.Pending || t.Status == Status.Scheduled) && t.ScheduledDate <= DateTime.Now.AddMonths(1)).ToList();
            if (transactionsFlagged.Any())
            {
                Console.WriteLine("Some transactions exist that should be posted or pending.");
                string verificationPrompt = Utilities.PromptYesOrNo("Would you like to verify necessary transactions?");
                if (verificationPrompt == "y")
                {
                    AppScreen.DisplayPostingOptions();
                    switch (Validator.Convert<int>("an option"))
                    {
                        case 1:
                            PostAllFlaggedTransactions(transactionsFlagged);
                            break;
                        case 2:
                            IndividuallySelectTransactionStatus(transactionsFlagged);
                            break;
                        case 3:
                            break;
                        default:
                            Utilities.PrintMessage("Invalid Option. Try again", false);
                            VerifyTransactionStatus();
                            break;
                    }
                }
                else
                {
                    Utilities.PrintMessage("Transactions left unchanged", true, false);
                }
            }
            else
            {
                Utilities.PrintMessage("No flagged transactions!", true, false);
            }
        }

        public void AddSingleTransaction(BudgetItem item, BudgetItemType type)
        {

            Transaction transaction = ConstructSingleTransaction(item, type);
            var postedDateString = transaction.Status == Status.Posted ? transaction.PostedDate.ToString() : "N/A";
            if (postedDateString != "N/A" && transaction.PostedDate != null)
            {
                postedDateString = ((DateTime)transaction.PostedDate).ToString("MMMM dd, yyyy");
            }
            var formattedAmount = Utilities.FormatAmount(transaction.Amount);
            var scheduledDate = transaction.ScheduledDate.ToString("MMMM dd yyyy");
            selectedAccount.TransactionList.Add(transaction);

            Utilities.PrintMessage($"Successfully created transaction with " +
                $"name: {transaction.Name}, " +
                $"amount: {formattedAmount}, " +
                $"budget item id: {transaction.BudgetItemId}, " +
                $"id: {transaction.Id}, " +
                $"Category Id: {transaction.CategoryId}, " +
                $"Created Date: {transaction.CreatedDate}, " +
                $"Type: {transaction.BudgetItemType}, " +
                $"Scheduled Date: {scheduledDate}, " +
                $"Posted Date: {postedDateString}!", true, false);
        }

        public void CreateMultipleTransactions(BudgetItem item, BudgetItemType type)
        {
            var curDate = item.StartDate;
            var daysBetweenTransactions = 0;

            switch (item.Rate)
            {
                case Rate.Weekly:
                    daysBetweenTransactions = 7;
                    break;
                case Rate.Biweekly:
                    daysBetweenTransactions = 14;
                    break;
                case Rate.Monthly:
                    break;
                case Rate.Yearly:
                    daysBetweenTransactions = 365;
                    break;
                case Rate.NoRate:
                case Rate.Other:
                    Console.WriteLine($"Cannot predict rate for other or no rate");
                    break;
                default:
                    throw new Exception();
            }
            if (item.EndDate != null)
            {
                while (curDate < ((DateTime)item.EndDate).AddMicroseconds(1))
                {
                    var transaction = new Transaction();
                    var postedDateString = string.Empty;
                    transaction.Id = AssignTransactionId();
                    transaction.Name = item.Name;
                    transaction.CategoryId = item.CategoryId;
                    transaction.Amount = item.Amount;
                    transaction.BudgetItemId = item.Id;
                    transaction.BudgetItemType = type;
                    transaction.CreatedDate = DateTime.Now;
                    transaction.ScheduledDate = (DateTime)curDate;

                    if (item.AmountVariable)
                    {
                        Console.WriteLine($"Name: {item.Name}, Date: {curDate:MMMM dd yyyy}, BudgetItemId: {item.Id}, Current Amount: {item.Amount}, Budget Item Type: {type}");
                        string isDifferentPrompt = Utilities.PromptYesOrNo($"Is this transaction different than {item.Amount}?");
                        if (isDifferentPrompt == "y")
                        {
                            transaction.Amount = Validator.Convert<decimal>("amount for this transaction");
                        }
                    }
                    if (item.Rate == Rate.Monthly)
                    {
                        daysBetweenTransactions = DateTime.DaysInMonth(((DateTime)curDate).Year, ((DateTime)curDate).Month);
                    }
                    if (curDate <= DateTime.Now)
                    {
                        transaction.PostedDate = DateTime.Now;
                        transaction.Status = Status.Posted;
                        postedDateString = ((DateTime)transaction.PostedDate).ToString("MMMM dd yyyy");
                    }
                    else
                    {
                        transaction.PostedDate = null;
                        transaction.Status = Status.Scheduled;
                        postedDateString = "Not Posted Yet";
                    }
                    selectedAccount.TransactionList.Add(transaction);

                    curDate = ((DateTime)curDate).AddDays(daysBetweenTransactions);

                    Utilities.PrintMessage($"Successfully created transaction! Id: " +
                        $"{transaction.Id}, " +
                        $"Name: {transaction.Name}, " +
                        $"Category Id: {transaction.CategoryId}, " +
                        $"Amount: {transaction.Amount}, " +
                        $"Budget Item Id: {transaction.BudgetItemId}," +
                        $"Budget Item Type: {transaction.BudgetItemType}," +
                        $"Created Date: {transaction.CreatedDate}," +
                        $"Scheduled Date: {transaction.ScheduledDate:MMMM dd yyyy}," +
                        $"PostedDate: {postedDateString}," +
                        $"Status: {transaction.Status}", true, true);
                    Console.WriteLine();
                }
            }
        }

        private int AssignTransactionId()
        {
            selectedAccount.TransactionIdCounter++;
            return selectedAccount.TransactionIdCounter;
        }

        private void IndividuallySelectTransactionStatus(List<Transaction> transactionsFlagged)
        {
            foreach (var t in transactionsFlagged)
            {
                if ((t.Status == Status.Pending || t.Status == Status.Scheduled) && t.ScheduledDate <= DateTime.Now)
                {
                    string promptToPost = Utilities.PromptYesOrNo($"Post this transaction? " +
                        $"Id: {t.Id} " +
                        $"Category Id: {t.CategoryId} " +
                        $"Budget Item Id: {t.BudgetItemId} " +
                        $"Name: {t.Name} " +
                        $"Amount: {t.Amount} " +
                        $"Created Date: {t.CreatedDate} " +
                        $"Scheduled Date: {t.ScheduledDate} " +
                        $"Budget Item Type: {t.BudgetItemType}" +
                        $"Status: {t.Status}");
                    if (promptToPost == "y")

                    {
                        t.Status = Status.Posted;
                    }
                    else
                    {
                        t.Status = Status.Cancelled;
                        Utilities.PrintMessage("Transaction cancelled", false, true);
                    }
                }

                if (t.Status == Status.Scheduled && t.ScheduledDate <= DateTime.Now.AddMonths(1) && t.ScheduledDate > DateTime.Now)
                {
                    string promptToPending = Utilities.PromptYesOrNo($"Make this transaction pending? " +
                        $"Id: {t.Id} " +
                        $"Category Id: {t.CategoryId} " +
                        $"Budget Item Id: {t.BudgetItemId} " +
                        $"Name: {t.Name} " +
                        $"Amount: {t.Amount} " +
                        $"Created Date: {t.CreatedDate} " +
                        $"Scheduled Date: {t.ScheduledDate} " +
                        $"Budget Item Type: {t.BudgetItemType}" +
                        $"Status: {t.Status}");
                    if (promptToPending == "y")
                    {
                        t.Status = Status.Pending;
                    }
                }
            }
        }

        private void PostAllFlaggedTransactions(List<Transaction> transactionsFlagged)
        {
            foreach(var transaction in transactionsFlagged)
            {

                transaction.Status = Status.Posted;
                transaction.PostedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                Console.WriteLine($"transaction posted: " +
                    $"{transaction.Name}, " +
                    $"{transaction.Id}," +
                    $"Category Id: {transaction.CategoryId}, " +
                    $"Amount: {transaction.Amount}, " +
                    $"Budget Item Id: {transaction.BudgetItemId}," +
                    $"Budget Item Type: {transaction.BudgetItemType}," +
                    $"Created Date: {transaction.CreatedDate}," +
                    $"Scheduled Date: {transaction.ScheduledDate}," +
                    $"PostedDate: {transaction.PostedDate}," +
                    $"Status: {transaction.Status}");
            }
        }

        private Transaction ConstructSingleTransaction(BudgetItem item, BudgetItemType type)
        {
            if(item.Rate != Rate.NoRate)
            {
                throw new Exception("This method can only be used for budget items with no rate or an other rate");
            }
            var transaction = new Transaction();
            
            int id;
            string name = item.Name;
            decimal amount = item.Amount;
            int categoryId = item.CategoryId;
            int budgetItemId = item.Id;
            BudgetItemType budgetItemType = BudgetItemType.None;
            DateTime createdDate = DateTime.Now;
            Status status = Status.None;

            id = AssignTransactionId();

            string postedToday = Utilities.PromptYesOrNo("Is this transaction posted today?");

            if(postedToday == "y")
            {
                transaction.ScheduledDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                transaction.PostedDate = transaction.ScheduledDate;
                status = Status.Posted;
            }
            else
            {
                
                transaction.ScheduledDate = Utilities.ConstructDate();

                if(transaction.ScheduledDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                {
                    Utilities.PrintMessage($"transaction posted on previous date: {transaction.ScheduledDate:MMMM dd yyyy}!", true, false);
                    status = Status.Posted;
                    transaction.PostedDate = transaction.ScheduledDate;
                }
                else if (transaction.ScheduledDate <= DateTime.Now.AddMonths(1))
                {
                    Utilities.PrintMessage($"transaction pending for {transaction.ScheduledDate:MMMM dd yyyy}!", true, false);
                    status = Status.Pending;
                }
                else
                {
                    Utilities.PrintMessage($"transaction scheduled for {transaction.ScheduledDate:MMMM dd yyyy}!", true, false);
                    status = Status.Scheduled;
                }
            }

            if (item.StartDate != transaction.ScheduledDate && item.StartDate != null)
            {
                Utilities.PrintMessage("Item start date is not synced with transaction scheduled date", false, false);
                var syncStartDateAndScheduledDate = Utilities.PromptYesOrNo("Sync up dates?");
                if(syncStartDateAndScheduledDate == "y")
                {
                    var dateToUse = Utilities.GetUserInput("t to use transaction scheduled/posted date or i for item start date");

                    while(dateToUse != "t" && dateToUse != "i")
                    {
                        Utilities.PrintMessage("Invalid input, please try again", false, false);
                        dateToUse = Utilities.GetUserInput("t for transaction scheduled/posted date or i for item start date");
                    }
                    if(dateToUse == "t")
                    {
                        item.StartDate = transaction.ScheduledDate;
                        Utilities.PrintMessage($"both dates are now {item.StartDate:MMMM dd yyyy}", true, false);
                    }
                    else
                    {
                        transaction.ScheduledDate = (DateTime)item.StartDate;
                        if(transaction.ScheduledDate <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                        {
                            transaction.PostedDate = DateTime.Now;
                        }
                        Utilities.PrintMessage($"both dates are now {transaction.ScheduledDate:MMMM dd yyyy}", true, false);
                    }
                }
                else
                {
                    Utilities.PrintMessage("item start date and transaction scheduled date will remain unsynced", true, false);
                }
            }

            if (type == BudgetItemType.Income)
            {
                budgetItemType = BudgetItemType.Income;
            }
            else if(type == BudgetItemType.Expense)
            {
                budgetItemType = BudgetItemType.Expense;
            }

            transaction.Id = id;
            transaction.CategoryId = categoryId;
            transaction.BudgetItemId = budgetItemId;
            transaction.Name = name;
            transaction.Amount = amount;
            transaction.CreatedDate = createdDate;
            transaction.BudgetItemType = budgetItemType;
            transaction.Status = status;

            return transaction;
        }

        public void UpdateTransactionsForTimePeriod(int daysInPast = 183, int daysInFuture = 365)
        {
            List<Transaction> transactionsExpected = new();
            Rate rate = new();
            var currentTransactionDay = new DateTime();
            var daysBetweenTransactions = 0;
            var transactionsTable = new ConsoleTable("Id", "Category Id", "Budget Item Id", "Name", "Amount", "Created Date", "Scheduled Date", "Posted Date", "Budget Item Type", "Status");
            var lists = new List<IEnumerable<BudgetItem>>
            {
                selectedAccount.ExpenseList,
                selectedAccount.IncomeList
            };
            for(var i = 0; i < lists.Count; i++) {
                var list = lists[i];
                var budgetItemTypeString = "No Type";
                var budgetItemType = BudgetItemType.None;

                if (list == selectedAccount.ExpenseList) {
                    budgetItemTypeString = "Expense";
                    budgetItemType = BudgetItemType.Expense;
                }
                else if (list == selectedAccount.IncomeList)
                {
                    budgetItemTypeString = "Income";
                    budgetItemType = BudgetItemType.Income;
                }
                foreach (var item in list)
                {
                    transactionsExpected = new List<Transaction>();
                    rate = item.Rate;
                    if (item.StartDate != null)
                    {
                        currentTransactionDay = (DateTime)item.StartDate;
                        if(item.EndDate == null)
                        {
                            Console.WriteLine("Select an end date for this list of transactions");
                            var transactionListEndDate = Utilities.ConstructDate();
                            item.EndDate = transactionListEndDate;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Select a date for this list of transactions to start");
                        var transactionListStartDate = Utilities.ConstructDate();
                        Console.WriteLine("Select an end date for this list of transactions");
                        var transactionListEndDate = Utilities.ConstructDate();
                        item.StartDate = transactionListStartDate;
                        item.EndDate = transactionListEndDate;
                        currentTransactionDay = (DateTime)item.StartDate;
                    }
                    daysBetweenTransactions = 0;

                    switch (rate)
                    {
                        case Rate.Weekly:
                            daysBetweenTransactions = 7;
                            break;
                        case Rate.Biweekly:
                            daysBetweenTransactions = 14;
                            break;
                        case Rate.Other:
                            daysBetweenTransactions = Validator.Convert<int>("Amount of days between transactions");
                            break;
                        case Rate.Monthly:
                        case Rate.Yearly:
                            continue;
                        case Rate.NoRate:
                            Console.WriteLine($"Cannot predict rate for {item.Name} ");
                            continue;
                        default:
                            throw new Exception();
                    }

                    while (currentTransactionDay < DateTime.Now.AddDays(-1 * daysInPast))
                    {
                        if (rate == Rate.Weekly || rate == Rate.Biweekly || rate == Rate.Other)
                        {
                            currentTransactionDay = currentTransactionDay.AddDays(daysBetweenTransactions);
                        }
                        else if (rate == Rate.Yearly)
                        {
                            currentTransactionDay = currentTransactionDay.AddYears(1);
                        }
                        else if (rate == Rate.Monthly)
                        {
                            currentTransactionDay = currentTransactionDay.AddMonths(1);
                        }
                        else if (rate == Rate.NoRate)
                        {
                            break;
                        }
                        else
                        {
                            Utilities.PrintMessage("invalid rate", false, false);
                            break;
                        }
                    }
                    var status = currentTransactionDay <= DateTime.Now ? Status.Posted : currentTransactionDay <= DateTime.Now.AddMonths(1) ? Status.Pending : Status.Scheduled; 

                    while (currentTransactionDay < DateTime.Now.AddDays(daysInFuture))
                    {
                        var transaction = new Transaction()
                        {
                            Id = AssignTransactionId(),
                            CategoryId = item.CategoryId,
                            BudgetItemId = item.Id,
                            Name = item.Name,
                            Amount = item.Amount,
                            CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                            ScheduledDate = currentTransactionDay,
                            PostedDate = currentTransactionDay >= DateTime.Now ? currentTransactionDay : null,
                            BudgetItemType = budgetItemType,
                            Status = status
                        };
                        var statusString = status == Status.Posted ? "Posted" : status == Status.Pending ? "Pending" : "Scheduled";

                        var postedDateString = transaction.PostedDate == null ? "No Posted Date" : ((DateTime)transaction.PostedDate).ToString("MMMM dd yyyy");

                        transactionsExpected.Add(transaction);

                        transactionsTable.AddRow(transaction.Id, transaction.CategoryId, transaction.BudgetItemId, transaction.Name, Utilities.FormatAmount(transaction.Amount), transaction.CreatedDate, transaction.ScheduledDate.ToString("MMMM dd yyyy"), postedDateString, budgetItemTypeString, statusString );

                        if (rate == Rate.Biweekly || rate == Rate.Weekly)
                        {
                            currentTransactionDay = currentTransactionDay.AddDays(daysBetweenTransactions);
                        }
                        else if (rate == Rate.Yearly)
                        {
                            if (DateTime.IsLeapYear(currentTransactionDay.Year))
                            {
                                daysBetweenTransactions = 366;
                            }
                            currentTransactionDay = currentTransactionDay.AddDays(daysBetweenTransactions);
                        }
                        else if (rate == Rate.Monthly)
                        {
                            currentTransactionDay = currentTransactionDay.AddDays(DateTime.DaysInMonth(currentTransactionDay.Year, currentTransactionDay.Month));
                        }
                        else if (rate == Rate.Other || rate == Rate.NoRate)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    transactionsTable.AddRow("break", "", "", "", "", "", "", "", "", "");
                }
            }
            transactionsTable.Write();
            Console.WriteLine("'\n'\n'\n....\n'  '\n'  '\n'  '\n'  '");
        }

        public void DisplayAllTransactions()
        {
            var postedDateForTransaction = string.Empty;

            foreach(var transaction in selectedAccount.TransactionList)
            {
                postedDateForTransaction = transaction.PostedDate == null ? "N/A" : ((DateTime)transaction.PostedDate).ToString("MMMM dd yyyy");

                Console.WriteLine($"" +
                    $"Name: {transaction.Name}; " +
                    $"Amount: {transaction.Amount}; " +
                    $"Id: {transaction.Id}; " +
                    $"Category Id: {transaction.CategoryId}; " +
                    $"Scheduled Date: {transaction.ScheduledDate}; " +
                    $"Created Date: {transaction.CreatedDate}; " +
                    $"Type: {transaction.BudgetItemType}; " +
                    $"Scheduled Date: {transaction.ScheduledDate}; " +
                    $"Status: {transaction.Status}; " +
                    $"Posted Date: {postedDateForTransaction}; ");
            }
        }

        public void GetDatesForTransactions(BudgetItem item)
        {
            var listOfDates = new List<DateTime>();

            var endDay = Validator.Convert<int>("end day");
            var endMonth = Validator.Convert<int>("end month");
            var endYear = Validator.Convert<int>("end year");

            var endDate = new DateTime(endYear, endMonth, endDay);

            switch (item.Rate)
            {
                case Rate.Weekly:
                    break;
                case Rate.Biweekly:
                    break;
                case Rate.Monthly:
                    break;
                case Rate.Yearly:
                    break;
            }

            foreach (DateTime date in listOfDates)
            {
                Console.WriteLine(date.ToString("MMMM dd, yyyy"));
            }
        }

        public void VerifyTransactionsOutOfDateRange(BudgetItem item)
        {
            var transactionList = selectedAccount.TransactionList.Where(x => item.Id == x.BudgetItemId);
            foreach(var transaction in transactionList)
            {
                if(transaction.ScheduledDate < item.StartDate || transaction.ScheduledDate > item.EndDate)
                {
                    var deleteTransactionPrompt = Utilities.PromptYesOrNo("Transaction is out of this budget item's range. Delete this transaction?");
                    if (deleteTransactionPrompt == "y")
                    {
                        Utilities.PrintMessage($"Transaction removed: " +
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
                    else
                    {

                    }
                }
            }
        }
        private void ProcessBudgetItem(decimal amount)
        {
            Console.WriteLine("\nProcessing expense");
            Utilities.PrintDotAnimation();
            Console.WriteLine("");

            if (amount <= 0)
            {
                Utilities.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }

            if (PreviewUpdate(amount) == false)
            {
                Utilities.PrintMessage("You have cancelled your action", false);
                return;
            }
        }
    }
}

