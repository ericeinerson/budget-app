using System;
using System.Net.NetworkInformation;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;
using System.Linq;
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
            var dayOfMonthStartDate = 0;
            var changeDayOfMonthForTransactions = string.Empty;

            if (item.StartDate != null)
            {
                dayOfMonthStartDate = ((DateTime)item.StartDate).Day;
            }
            if(dayOfMonthStartDate > 28 && item.Rate == Rate.Monthly)
            {
                changeDayOfMonthForTransactions = Utilities.PromptYesOrNo("" +
                    "This day may change if transactions are done on a month with " +
                    "fewer days. Keep the same day on all months if possible?");
            }
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
                case Rate.Yearly:
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

                    if (item.AmountVariable)
                    {
                        Console.WriteLine($"Name: {item.Name}, Date: {curDate:MMMM dd yyyy}, BudgetItemId: {item.Id}, Current Amount: {Utilities.FormatAmount(item.Amount)}, Budget Item Type: {type}");
                        string isDifferentPrompt = Utilities.PromptYesOrNo($"Is this transaction different than {Utilities.FormatAmount(item.Amount)}?");
                        if (isDifferentPrompt == "y")
                        {
                            transaction.Amount = Validator.Convert<decimal>("amount for this transaction");
                        }
                    }
                    if(item.Rate == Rate.Weekly || item.Rate == Rate.Biweekly || item.Rate == Rate.Other)
                    {
                        curDate = ((DateTime)curDate).AddDays(daysBetweenTransactions);
                    }
                    else if (item.Rate == Rate.Monthly)
                    {
                        curDate = ((DateTime)curDate).AddMonths(1);
                    }
                    else if(item.Rate == Rate.Yearly)
                    {
                        curDate = ((DateTime)curDate).AddYears(1);
                    }

                    selectedAccount.TransactionList.Add(transaction);
                    if(transaction.ScheduledDate.Day != dayOfMonthStartDate && changeDayOfMonthForTransactions == "y")
                    {
                        curDate = new DateTime(((DateTime)curDate).Year, ((DateTime)curDate).Month, dayOfMonthStartDate);
                    }

                    Utilities.PrintMessage($"Successfully created transaction! Id: " +
                        $"{transaction.Id}, " +
                        $"Name: {transaction.Name}, " +
                        $"Category Id: {transaction.CategoryId}, " +
                        $"Amount: {Utilities.FormatAmount(transaction.Amount)}, " +
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
                        $"Amount: {Utilities.FormatAmount(t.Amount)} " +
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
                        $"Amount: {Utilities.FormatAmount(t.Amount)} " +
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
                    $"Amount: {Utilities.FormatAmount(transaction.Amount)}, " +
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

        public void DisplayAllTransactions()
        {
            var transactionsTable = new ConsoleTable("Id", "Budget Item Id", "Category Id", "Name", "Amount", "Scheduled Date", "Created Date", "Posted Date","Budget Item Type", "Status");
            var postedDateForTransaction = string.Empty;

            foreach(var transaction in selectedAccount.TransactionList)
            {
                postedDateForTransaction = (transaction.PostedDate == null || transaction.PostedDate == DateTime.MinValue) ? "N/A" : ((DateTime)transaction.PostedDate).ToString("MMMM dd yyyy");

                transactionsTable.AddRow(transaction.Id, transaction.BudgetItemId, transaction.CategoryId, transaction.Name, Utilities.FormatAmount(transaction.Amount), transaction.ScheduledDate, transaction.CreatedDate, postedDateForTransaction, transaction.BudgetItemType, transaction.Status);

            }
            transactionsTable.Write();
            Utilities.PressEnterToContinue();
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
            var transactionList = selectedAccount.TransactionList.Where(x => item.Id == x.BudgetItemId).ToList();
            var transactionsToRemoveList = new List<Transaction>();

            foreach(Transaction transaction in transactionList)
            {
                if(transaction.ScheduledDate < item.StartDate || transaction.ScheduledDate > item.EndDate)
                {
                    var deleteTransactionPrompt = Utilities.PromptYesOrNo($"The following transaction is out of this budget item's range:" +
                        $"Id: {transaction.Id}; " +
                        $"Budget Item Id: {transaction.BudgetItemId};" +
                        $"Name: {transaction.Name};" +
                        $"Amount: {Utilities.FormatAmount(transaction.Amount)}; " +
                        $"Scheduled Date: {transaction.ScheduledDate} " +
                        $"Delete this transaction?");
                    if (deleteTransactionPrompt == "y")
                    {
                        Utilities.PrintMessage($"Transaction removed: " +
                            $"Id: {transaction.Id}; " +
                            $"Category Id: {transaction.Id}; " +
                            $"Budget Item Id: {transaction.BudgetItemId}; " +
                            $"Name: {transaction.Name}; " +
                            $"Amount: {Utilities.FormatAmount(transaction.Amount)}; " +
                            $"Created Date {transaction.CreatedDate}; " +
                            $"Scheduled Date {transaction.ScheduledDate}; " +
                            $"Deleted Date: {DateTime.Now}; " +
                            $"Posted Date: {transaction.PostedDate:MMMM dd yyyy}; " +
                            $"Budget Item Type: {transaction.BudgetItemType} " +
                            $"Status: {Status.Cancelled}", true, false);
                        transactionsToRemoveList.Add(transaction);
                    }
                    else
                    {
                        Utilities.PrintMessage($"Transaction left outside {item.Name}'s range: " +
                            $"Id: {transaction.Id}; " +
                            $"Category Id: {transaction.Id}; " +
                            $"Budget Item Id: {transaction.BudgetItemId}; " +
                            $"Name: {transaction.Name}; " +
                            $"Amount: {Utilities.FormatAmount(transaction.Amount)}; " +
                            $"Created Date {transaction.CreatedDate}; " +
                            $"Deleted Date: {DateTime.Now}; " +
                            $"Posted Date: {transaction.PostedDate:MMMM dd yyyy}; " +
                            $"Budget Item Type: {transaction.BudgetItemType} " +
                            $"Status: {Status.Cancelled}", false, false);
                    }
                    continue;
                }
                foreach (Transaction transactionRemoved in transactionsToRemoveList)
                {
                    selectedAccount.TransactionList.Remove(transactionRemoved);
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

        public void ValidateTransactionsForTimePeriod(int daysInPast = 180, int daysInFuture = 365)
        {
            Rate rate;
            DateTime startDay;
            DateTime endDay;
            var transactionsTable = new ConsoleTable("Row Number","Name", "Id", "Category Id", "Budget Item Id", "Amount", "Created Date", "Scheduled Date", "Posted Date", "Budget Item Type", "Status");
            var lists = new List<IEnumerable<BudgetItem>>
            {
                selectedAccount.ExpenseList,
                selectedAccount.IncomeList
            };
            for (var i = 0; i < lists.Count; i++)
            {
                var list = lists[i];
                var budgetItemTypeString = "No Type";
                var budgetItemType = BudgetItemType.None;

                if (list == selectedAccount.ExpenseList)
                {
                    budgetItemTypeString = "Expense";
                    budgetItemType = BudgetItemType.Expense;
                }
                else if (list == selectedAccount.IncomeList)
                {
                    budgetItemTypeString = "Income";
                    budgetItemType = BudgetItemType.Income;
                }
                transactionsTable.AddRow($"{budgetItemTypeString} Transactions", "", "", "", "", "", "", "", "", "", "");

                foreach (var item in list)
                {
                    startDay = DateTime.Now.AddDays(-1 * daysInPast);
                    endDay = DateTime.Now.AddDays(daysInFuture);

                    var transactionsExpected = new List<Transaction>();

                    rate = item.Rate;
                    if (item.Rate == Rate.NoRate)
                    {
                        Utilities.PrintMessage($"Following item does not have a rate, so no transactions to valdate:" +
                            $"Id: {item.Id} " +
                            $"Name: {item.Name} " +
                            $"Amount: {item.Amount}", true, false);
                    }
                    else
                    {
                        if (item.StartDate == null || item.EndDate == null)
                        {
                            Utilities.PrintMessage($"Cannot validate transactions for this item " +
                                $"Name: {item.Name} " +
                                $"Id: {item.Id} " +
                                $"Amount: {item.Amount} " +
                                $"because rate exists but either start date " +
                                $"or end date is null", false, false);
                        }
                        else
                        {
                            var rowCounter = 0;
                            var dateTimesForItem = GetExpectedTransactionDateTimeList(startDay, endDay, item);
                            if (dateTimesForItem.Any()) {
                                foreach (var date in dateTimesForItem)
                                {
                                    Transaction? matchedTransaction = MatchingTransaction(item, date);

                                    if (matchedTransaction != null)
                                    {
                                        continue;
                                    }

                                    var status = date <= DateTime.Now ? Status.Posted : date <= DateTime.Now.AddMonths(1) ? Status.Pending : Status.Scheduled;

                                    var transaction = new Transaction()
                                    {
                                        Id = AssignTransactionId(),
                                        CategoryId = item.CategoryId,
                                        BudgetItemId = item.Id,
                                        Name = item.Name,
                                        Amount = item.Amount,
                                        CreatedDate = DateTime.Now,
                                        ScheduledDate = date,
                                        PostedDate = date <= DateTime.Now ? date : null,
                                        BudgetItemType = budgetItemType,
                                        Status = status
                                    };
                                    var statusString = status == Status.Posted ? "Posted" : status == Status.Pending ? "Pending" : "Scheduled";

                                    var postedDateString = transaction.PostedDate == null ? "No Posted Date" : ((DateTime)transaction.PostedDate).ToString("MMMM dd yyyy");

                                    transactionsExpected.Add(transaction);

                                    rowCounter++;

                                    transactionsTable.AddRow(rowCounter, transaction.Name, transaction.Id, transaction.CategoryId, transaction.BudgetItemId, Utilities.FormatAmount(transaction.Amount), transaction.CreatedDate, transaction.ScheduledDate.ToString("MMMM dd yyyy"), postedDateString, budgetItemTypeString, statusString);
                                }
                                transactionsTable.AddRow("break", "", "", "", "", "", "", "", "", "", "");
                            }
                        }
                    }
                    foreach (Transaction transaction in transactionsExpected)
                    {
                        selectedAccount.TransactionList.Add(transaction);
                    }
                }
            }
            Console.WriteLine("Transactions added \n\n -----------------------");
            transactionsTable.Write();
        }
         
        public int CalculateDayOffsetBasedOnDaysBetweenTransactions(int totalDays, int daysBetweenTransactions)
        {
            var daysOffset = 0;
            daysOffset = totalDays % daysBetweenTransactions;
                    
            return daysOffset;
        }

        public int CalculateDayOffsetBasedOnMonthlyOrYearlyRate(int totalDays, Rate rate, DateTime? curDay = null, string pastOrFuture = "past")
        {
            DateTime endDay;
            var daysOffset = 0;

            while (pastOrFuture != "past" && pastOrFuture != "future")
            {
                Utilities.PrintMessage("Invalid past or future option", false, true);
                var pastOrFutureOption = Utilities.GetUserInput("past, p, future, or f");
                if(pastOrFutureOption == "p" || pastOrFutureOption.ToLower() == "past")
                {
                    pastOrFuture = "past";
                }
                if (pastOrFutureOption == "f" || pastOrFutureOption.ToLower() == "future")
                {
                    pastOrFuture = "future";
                }
            }
            if(curDay == null)
            {
                curDay = DateTime.Now;
            }
            
            var adjustedDayOfTransaction = ((DateTime)curDay).Day;

            if (pastOrFuture == "past")
            {
                endDay = ((DateTime)curDay).AddDays(-1 * totalDays);

                if (rate == Rate.Monthly)
                {
                    while (endDay < ((DateTime)curDay).AddMonths(-1))
                    {
                        if(((DateTime)curDay).Day != adjustedDayOfTransaction)
                        {
                            var correctedDayOfMonth = AdjustDateTimeBasedOnDayOfMonth(adjustedDayOfTransaction, (DateTime)curDay);
                            curDay = new DateTime(((DateTime)curDay).Year, ((DateTime)curDay).Month, correctedDayOfMonth);
                        }
                        curDay = ((DateTime)curDay).AddMonths(-1);
                    }
                }
                else if (rate == Rate.Yearly)
                {
                    while (endDay < ((DateTime)curDay).AddYears(-1))
                    {
                        curDay = ((DateTime)curDay).AddYears(-1);
                    }
                }
                else
                {
                    throw new Exception();
                }

                daysOffset = ((DateTime)curDay - endDay).Days;
            }
            else if(pastOrFuture == "future")
            {
                endDay = ((DateTime)curDay).AddDays(totalDays);

                if(rate == Rate.Monthly)
                {
                    while (endDay > ((DateTime)curDay).AddMonths(1))
                    {
                        if (((DateTime)curDay).Day != adjustedDayOfTransaction)
                        {
                            AdjustDateTimeBasedOnDayOfMonth(adjustedDayOfTransaction, (DateTime)curDay);
                        }
                        curDay = ((DateTime)curDay).AddMonths(1);
                    }
                }
                else if (rate == Rate.Yearly)
                {
                    while (endDay > ((DateTime)curDay).AddYears(1))
                    {
                        curDay = ((DateTime)curDay).AddYears(1);
                    }
                }
                else
                {
                    throw new Exception();
                }

                daysOffset = (endDay - (DateTime)curDay).Days;
            }

            return daysOffset;
        }

        public int CalculateDayOffsetBasedOnYearlyRate()
        {
            return 0;
        }

        public Transaction? MatchingTransaction(BudgetItem item, DateTime transactionDate)
        {
            var itemTransaction = selectedAccount.TransactionList.FirstOrDefault(x => x.ScheduledDate == transactionDate && item.Id == x.BudgetItemId);

            return itemTransaction;
        }

        public int AdjustDateTimeBasedOnDayOfMonth(int adjustedDayOfMonth, DateTime curDate)
        {
            var dayAdjusted = adjustedDayOfMonth;

            switch (curDate.Month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    if(adjustedDayOfMonth == 31)
                    {
                        dayAdjusted = 30;
                    }
                    break;
                case 2:
                    if (DateTime.IsLeapYear(curDate.Year))
                    {
                        dayAdjusted = 29;
                    }
                    else
                    {
                        dayAdjusted = 28;
                    }
                    break;
            }

            return dayAdjusted;
        }

        public List<DateTime> GetExpectedTransactionDateTimeList(DateTime dateTimeCur, DateTime dateRangeEnd, BudgetItem item)
        {
            var daysBetweenTransactions = 0;
            var listOfDatesForItem = new List<DateTime>();
            var adjustedDayOfMonth = 0;
             
            switch (item.Rate)
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
                    if (item.StartDate != null)
                    {
                        if (((DateTime)item.StartDate).Day > 28)
                        {
                            adjustedDayOfMonth = ((DateTime)item.StartDate).Day;
                        }
                    }
                    break;
                case Rate.Yearly:
                    break;
                default:
                    throw new Exception();
            }

            if(dateTimeCur <= item.StartDate)
            {
                dateTimeCur = (DateTime)item.StartDate;
            }
            else if(item.StartDate != null)
            {
                var daysOffset = (dateTimeCur - (DateTime)item.StartDate).Days;
                if(item.Rate == Rate.Weekly || item.Rate == Rate.Biweekly)
                {
                    var daysToShift = daysOffset % daysBetweenTransactions;
                    dateTimeCur = dateTimeCur.AddDays(daysBetweenTransactions - daysToShift);
                }
                else if(item.Rate == Rate.Monthly)
                {
                    var dayOfMonth = ((DateTime)item.StartDate).Day;
                    var dateRangeStartDayOfMonth = DateTime.DaysInMonth(dateTimeCur.Year, dateTimeCur.Month);
                    dateRangeStartDayOfMonth = Math.Min(dateRangeStartDayOfMonth, dayOfMonth);
                    dateTimeCur = new DateTime(dateTimeCur.Year, dateTimeCur.Month, dateRangeStartDayOfMonth);
                }
                else if(item.Rate == Rate.Yearly)
                {
                    var dateIncrementorYear = (DateTime)item.StartDate;
                    while(dateTimeCur > dateIncrementorYear)
                    {
                        dateIncrementorYear = dateIncrementorYear.AddYears(1);
                    }
                    dateTimeCur = dateIncrementorYear;
                }
            }

            while(dateTimeCur < dateRangeEnd && dateTimeCur < item.EndDate)
            {
                listOfDatesForItem.Add(new DateTime(dateTimeCur.Year, dateTimeCur.Month, dateTimeCur.Day));

                if (item.Rate == Rate.Weekly || item.Rate == Rate.Biweekly)
                {
                    dateTimeCur = dateTimeCur.AddDays(daysBetweenTransactions);
                }
                else if (item.Rate == Rate.Monthly)
                {
                    dateTimeCur = dateTimeCur.AddMonths(1);

                    if (adjustedDayOfMonth != 0)
                    {
                        var correctedDayOfMonth = AdjustDateTimeBasedOnDayOfMonth(adjustedDayOfMonth, dateTimeCur);
                        dateTimeCur = new DateTime(dateTimeCur.Year, dateTimeCur.Month, correctedDayOfMonth);
                    }
                }
                else if (item.Rate == Rate.Yearly)
                {
                    dateTimeCur = dateTimeCur.AddYears(1);
                }
            }

            return listOfDatesForItem;
        }
    }
}

