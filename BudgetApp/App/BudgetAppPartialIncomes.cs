using System;
using System.Globalization;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;

namespace BudgetApp.App
{
	public partial class BudgetApp
	{

        private void ProcessIncomeMenuOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case (int)IncomeOption.ViewIncomes:
                    ViewIncomes();
                    break;
                case (int)IncomeOption.AddIncome:
                    AddIncome();
                    break;
                case (int)IncomeOption.RemoveIncome:
                    RemoveIncome();
                    break;
                case (int)IncomeOption.UpdateIncomeDetails:
                    UpdateIncomeDetails();
                    break;
                case (int)IncomeOption.Logout:
                    AppScreen.LogoutProgress();
                    Run();
                    break;
                case (int)IncomeOption.GoBack:
                    GoBackToAppScreen();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessWishlistOption();
                    break;
            }
        }

        public void ViewIncomes()
        {
            ConsoleTable allIncomesTable = new ConsoleTable("Name", "Amount", "Date", "Id", "Category");

            foreach (Income income in selectedAccount.IncomeList)
            {
                TransactionCategory? category = selectedAccount.TransactionCategoryList.Find(t => t.Id == income.CategoryId);
                var categoryName = category != null ? category.Name : string.Empty;
                allIncomesTable.AddRow(income.IncomeName, income.AmountFormatted, income.Date.ToString("MMMM dd, yyyy"), income.Id, categoryName);
            }
            allIncomesTable.Write();
            Utilities.PressEnterToContinue();
        }

        public void AddIncome()
        {
            Income income = ConstructIncome();
            selectedAccount.IncomeList.Add(income);

            Utilities.PrintMessage($"You have succcessfully added {income.IncomeName} with a value of {income.AmountFormatted}. This income will be done on {income.Date.ToString("MMMM dd, yyyy")}!", true, false);
        }

        public void RemoveIncome()
        {
            Income income = FindIncome();
            if (income != null)
            {
                selectedAccount.IncomeList.Remove(income);
                Utilities.PrintMessage($"You have succcessfully removed {income.IncomeName} with a value of {income.AmountFormatted}!", true, false);
            }
        }

        public Income FindIncome()
        {
            Income? income = null;

            while (income == null)
            {
                string incomeName = Utilities.GetUserInput("income name. If not known, enter n to skip or a to exit to app menu").ToLower();
                if (incomeName == "a")
                {
                    break;
                }
                income = selectedAccount.IncomeList.FirstOrDefault(e => e.IncomeName.ToLower() == incomeName.ToLower());
                if (incomeName == "n")
                {
                    decimal incomeAmount = Validator.Convert<decimal>("income amount");
                    income = selectedAccount.IncomeList.FirstOrDefault(e => e.Amount == incomeAmount);
                }

                if (income == null)
                {
                    Utilities.PrintMessage("Sorry, income not found. Please try again", false, false);
                }
            }

            if (income == null)
            {
                throw new NullReferenceException();

            }
            return income;
        }

        public Income ConstructIncome()
        {
            Income income = new Income();
            selectedAccount.IncomeId++;
            int id = selectedAccount.IncomeId;
            string name = Utilities.GetUserInput("income name");
            decimal amount = Validator.Convert<decimal>("income amount");
            string formattedAmount = string.Format(new CultureInfo("en-US"), "{0:c}", amount);
            TransactionCategory category = AssignTransactionCategory();

            Rate rate = ProcessRateOption();
            DateTime date = DateTime.MinValue;
            string todayOrFuture = Utilities.GetUserInput("whether t for today or f for future").ToLower();

            if (todayOrFuture == "t")
            {
                date = DateTime.Now;
            }
            else if (todayOrFuture == "f")
            {
                date = Utilities.ConstructDate();
            }

            income.Id = id;
            income.IncomeName = name;
            income.Amount = amount;
            income.AmountFormatted = formattedAmount;
            income.Rate = rate;
            income.Date = date;
            income.CategoryId = category.Id;

            return income;
        }

        private void UpdateIncomeDetails()
        {
            var income = FindIncome();

            AppScreen.DisplayIncomeUpdateDetails();

            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    UpdateIncomeAmount(income);
                    break;
                case 2:
                    UpdateIncomeName(income);
                    break;
                case 3:
                    UpdateIncomeRate(income);
                    break;
                case 4:
                    UpdateIncomeDate(income);
                    break;
                case 5:
                    UpdateIncomeCategory(income);
                    break;
                case 6:
                    UpdateAllIncomeDetails(income);
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    UpdateIncomeDetails();
                    break;
            }
        }

        private void UpdateIncomeAmount(Income income)
        {
            var amount = Validator.Convert<decimal>("new amount");
            income.Amount = amount;
            income.AmountFormatted = string.Format(new CultureInfo("en-US"), "{0:c}", amount);
        }

        private void UpdateIncomeName(Income income)
        {
            var name = Utilities.GetUserInput("new name");
            income.IncomeName = name;
        }

        private void UpdateIncomeRate(Income income)
        {
            var rate = ProcessRateOption();
            income.Rate = rate;
        }

        private void UpdateIncomeDate(Income income)
        {
            var date = Utilities.ConstructDate();
            income.Date = date;
        }

        private void UpdateIncomeCategory(Income income)
        {
            var categoryId = AssignTransactionCategory().Id;
            income.CategoryId = categoryId;
        }

        private void UpdateAllIncomeDetails(Income income)
        {
            UpdateIncomeAmount(income);
            UpdateIncomeName(income);
            UpdateIncomeRate(income);
            UpdateIncomeDate(income);
            UpdateIncomeCategory(income);
        }

        void CalculateIncomesForEachRate()
        {
            decimal weeklyIncomes = 0;
            decimal biweeklyIncomes = 0;
            decimal monthlyIncomes = 0;
            decimal yearlyIncomes = 0;

            foreach (Income income in selectedAccount.IncomeList)
            {
                switch (income.Rate)
                {
                    case Rate.Weekly:
                        weeklyIncomes += income.Amount;
                        break;
                    case Rate.Biweekly:
                        biweeklyIncomes += income.Amount;
                        break;
                    case Rate.Monthly:
                        monthlyIncomes += income.Amount;
                        break;
                    case Rate.Yearly:
                        yearlyIncomes += income.Amount;
                        break;
                }
            }
        }

        //private void UpdateIncome()
        //{
        //    int selectedIncomeId = Validator.Convert<int>("Enter Id of income to update");
        //    try
        //    {
        //        selectedAccount.IncomeList.Find(i => i.Id == selectedIncomeId);
        //    }
        //    catch
        //    {
        //        Utilities.PrintMessage("Id could not be found. Please try again.", false);
        //        UpdateIncome();
        //    }
        //    AppScreen.DisplayIncomeUpdateOptions();
        //    ProcessIncomeChangeOption(selectedIncomeId, GetSelectedAccount());
        //}

        //private void ProcessIncomeChangeOption(int id, UserAccount selectedAccount)
        //{
        //    switch (Validator.Convert<int>("an option"))
        //    {
        //        case 1:
        //            Console.WriteLine("Name");
        //            selectedAccount.IncomeList.Find(i => i.Id == id).IncomeName = Utilities.GetUserInput("new name");
        //            break;
        //        case 2:
        //            Console.WriteLine("Amount");
        //            Income selectedIncome = selectedAccount.IncomeList.Find(i => i.Id == id);
        //            selectedIncome.Amount = Validator.Convert<decimal>("new amount");
        //            InsertTransaction(selectedAccount.Id, TransactionType.Income, selectedIncome.Amount, $"updated amount of income {selectedIncome.IncomeName}");
        //            break;
        //        case 3:
        //            Console.WriteLine("Rate");
        //            AppScreen.DisplayRateOptions();
        //            selectedAccount.IncomeList.Find(i => i.Id == id).Rate = ProcessRateOption();
        //            break;
        //        case 4:
        //            AppScreen.LogoutProgress();
        //            Utilities.PrintMessage("You have successfully logged out.", true);
        //            Run();
        //            break;
        //        default:
        //            Utilities.PrintMessage("Invalid Option. Try again", false);
        //            ProcessIncomeUpdateOption();
        //            break;
        //    }

        //}

        //decimal CalculateIncomesForTimePeriod(TimeRange timeRange, DateTime endTime)
        //{
        //    allIncomes = 0;

        //    foreach (Income income in selectedAccount.IncomeList)
        //    {
        //        allIncomes += CalculateIncomeByRateAndTime(timeRange, income, endTime);
        //    }

        //    CalculateIncomesForEachRate();

        //    return allIncomes;
        //}

        //decimal CalculateIncomesForTimePeriod(TimeRange timeRange)
        //{
        //    allIncomes = 0;

        //    foreach (Income income in selectedAccount.IncomeList)
        //    {
        //        allIncomes += CalculateIncomeByRateAndTime(timeRange, income);
        //    }

        //    CalculateIncomesForEachRate();

        //    return allIncomes;
        //}

    }
}

