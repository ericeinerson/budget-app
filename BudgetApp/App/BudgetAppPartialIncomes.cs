using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;

namespace BudgetApp.App
{
	public partial class BudgetApp
	{
        //public void ManageIncome()
        //{
        //    CalculateIncomesForEachRate();

        //    Console.WriteLine($"\nSum of weekly incomes: {Utilities.FormatAmount(weeklyIncomes)}\n");
        //    Console.WriteLine($"\nSum of biweekly incomes: {Utilities.FormatAmount(biweeklyIncomes)}\n");
        //    Console.WriteLine($"\nSum of monthly incomes: {Utilities.FormatAmount(monthlyIncomes)}\n");
        //    Console.WriteLine($"\nSum of yearly incomes: {Utilities.FormatAmount(yearlyIncomes)}\n");

        //    Utilities.PressEnterToContinue();
        //    AppScreen.DisplayIncomeMenu();

        //    ProcessIncomeUpdateOption();

        //    Utilities.PressEnterToContinue();
        //}

        //void CalculateIncomesForEachRate()
        //{
        //    weeklyIncomes = 0;
        //    biweeklyIncomes = 0;
        //    monthlyIncomes = 0;
        //    yearlyIncomes = 0;

        //    foreach (Income income in selectedAccount.IncomeList)
        //    {
        //        switch (income.Rate)
        //        {
        //            case Rate.Weekly:
        //                weeklyIncomes += income.Amount;
        //                break;
        //            case Rate.Biweekly:
        //                biweeklyIncomes += income.Amount;
        //                break;
        //            case Rate.Monthly:
        //                monthlyIncomes += income.Amount;
        //                break;
        //            case Rate.Yearly:
        //                yearlyIncomes += income.Amount;
        //                break;
        //        }
        //    }
        //}

        //private void ProcessIncomeUpdateOption()
        //{
        //    switch (Validator.Convert<int>("an option"))
        //    {
        //        case 1:
        //            Console.WriteLine("View Incomes");
        //            ViewIncome();
        //            break;
        //        case 2:
        //            Console.WriteLine("Add new income");
        //            AddNewIncome();
        //            break;
        //        case 3:
        //            Console.WriteLine("Update an income\n\n");
        //            UpdateIncome();
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

        //private void ViewIncome()
        //{
        //    foreach (Income i in selectedAccount.IncomeList)
        //    {
        //        Console.WriteLine($"Income: {i.IncomeName}, Amount: {Utilities.FormatAmount(i.Amount)}, Frequency/Rate: {i.Rate}, Id: {i.Id}");
        //    }
        //    Utilities.PressEnterToContinue();
        //}

        //private void AddNewIncome()
        //{
        //    _incomeIdCounter++;

        //    string incomeName = Utilities.GetUserInput("income name");
        //    decimal incomeAmount = Validator.Convert<decimal>("income amount");
        //    int monthDeposited = Validator.Convert<int>("month of each deposit");
        //    int dayDeposited = Validator.Convert<int>("day of each deposit");
        //    DateTime dateDeposited = new DateTime(DateTime.Now.Year, monthDeposited, dayDeposited);
        //    AppScreen.DisplayRateOptions();
        //    Rate incomeRate = ProcessRateOption();

        //    selectedAccount.IncomeList.Add(new Income { IncomeName = incomeName, Amount = incomeAmount, Day = dayDeposited, Month = monthDeposited, Rate = incomeRate, Id = _incomeIdCounter });

        //    ConsoleTable newIncomeTbl = new ConsoleTable("Name", "Amount", "Start Date", "Rate Of Deposit");
        //    newIncomeTbl.AddRow(incomeName, incomeAmount, dateDeposited.ToString("MMMM dd"), incomeRate);
        //    newIncomeTbl.Options.EnableCount = false;
        //    newIncomeTbl.Write();

        //    InsertTransaction(selectedAccount.Id, TransactionType.Income, incomeAmount, $"added new income {incomeName} to list of incomes");
        //}

        //private Rate ProcessRateOption()
        //{
        //    switch (Validator.Convert<int>("an option"))
        //    {
        //        case 1:
        //            Console.WriteLine("Weekly");
        //            return Rate.Weekly;
        //        case 2:
        //            Console.WriteLine("Biweekly");
        //            return Rate.Biweekly;
        //        case 3:
        //            Console.WriteLine("Monthly");
        //            return Rate.Monthly;
        //        case 4:
        //            Console.WriteLine("Yearly");
        //            return Rate.Yearly;
        //        case 5:
        //            AppScreen.LogoutProgress();
        //            Utilities.PrintMessage("You have successfully logged out.", true);
        //            Run();
        //            return Rate.Other;
        //        default:
        //            Utilities.PrintMessage("Invalid Option. Try again", false);
        //            ProcessIncomeUpdateOption();
        //            return Rate.Other;
        //    }
        //}

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

        //private UserAccount GetSelectedAccount()
        //{
        //    return selectedAccount;
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

        //decimal CalculateExpensesForTimePeriod(TimeRange timeRange, DateTime endTime)
        //{

        //    SumOfAllExpenses = 0;

        //    foreach (Expense expense in selectedAccount.ExpenseList)
        //    {
        //        SumOfAllExpenses += CalculateExpenseByRateAndTime(timeRange, expense, endTime);
        //    }

        //    CalculateExpensesForEachRate();

        //    return SumOfAllExpenses;
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

