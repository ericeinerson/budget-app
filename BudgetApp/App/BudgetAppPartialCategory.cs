using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;
using ConsoleTables;

namespace BudgetApp.App
{
	public partial class BudgetApp
	{
        public void ProcessCategoryMenuOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case (int)CategoryOption.ViewCategories:
                    ViewTransactionCategories();
                    break;
                case (int)CategoryOption.AddCategory:
                    AddTransactionCategory();
                    break;
                case (int)CategoryOption.RemoveCategory:
                    RemoveTransactionCategory();
                    break;
                case (int)CategoryOption.UpdateCategoryDetails:
                    UpdateCategoryDetails();
                    break;
                case (int)CategoryOption.Logout:
                    LogoutProgress();
                    break;
                case (int)CategoryOption.GoBack:
                    GoBackToAppScreen();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessCategoryMenuOption();
                    break;
            }
        }

        public void AddTransactionCategory()
        {
            Category category = ConstructTransactionCategory();
            selectedAccount.CategoryList.Add(category);

            Utilities.PrintMessage($"You have succcessfully added {category.Name} with an id of {category.Id}!", true, false);
        }

        public void RemoveTransactionCategory()
        {
            Category category = FindTransactionCategory();
            if (category != null && category.Id != 0)
            {
                selectedAccount.CategoryList.Remove(category);
                foreach (BudgetItem item in selectedAccount.ExpenseList)
                {
                    if (item.CategoryId == category.Id)
                    {
                        item.CategoryId = 0;
                    }
                }

                foreach (BudgetItem item in selectedAccount.IncomeList)
                {
                    if (item.CategoryId == category.Id)
                    {
                        item.CategoryId = 0;
                    }
                }

                Utilities.PrintMessage($"You have succcessfully removed {category.Name} with an id of {category.Id}!", true, false);
            }
            else if (category != null && category.Id == 0)
            {
                Utilities.PrintMessage("You cannot remove this category. It serves as a replacement for holding no categories", false, false);
            }
        }

        public Category FindTransactionCategory()
        {
            Category? category = null;

            while (category == null)
            {
                string categoryName = Utilities.GetUserInput("category name. If not known, enter n to skip or a to exit to app menu").ToLower();
                if (categoryName == "a")
                {
                    break;
                }
                category = selectedAccount.CategoryList.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
                if (categoryName == "n")
                {
                    int categoryId = Validator.Convert<int>("category id");
                    category = selectedAccount.CategoryList.FirstOrDefault(c => c.Id == categoryId);
                }

                if (category == null)
                {
                    Utilities.PrintMessage("Sorry, category not found. Please try again", false, false);
                }
            }

            if(category == null)
            {
                throw new NullReferenceException();
            }

            return category;
        }

        public Category ConstructTransactionCategory()
        {
            Category category = new Category();

            string categoryName = Utilities.GetUserInput("category name");
            int categoryId = 0;

            foreach (Category tc in selectedAccount.CategoryList)
            {
                categoryId = Math.Max(categoryId, tc.Id);
            }
            categoryId++;

            category.Name = categoryName;
            category.Id = categoryId;

            return category;
        }

        public void ViewTransactionCategories()
        {
            ConsoleTable allCategoriesTable = new ConsoleTable("Name", "Id");
            foreach (Category category in selectedAccount.CategoryList)
            {
                allCategoriesTable.AddRow(category.Name, category.Id);
            }
            allCategoriesTable.Write();
            Utilities.PressEnterToContinue();
        }

        public Category AssignTransactionCategory()
        {
            Category? category = null;

            var categoryList = selectedAccount.CategoryList;
            string? categoryProperty = Utilities.GetUserInput("category name or id. Press q to quit");

            while (category == null)
            {
                category = categoryList.FirstOrDefault(c => c.Id.ToString() == categoryProperty);

                if (category == null)
                {
                    category = categoryList.FirstOrDefault(c => c.Name == categoryProperty);
                }

                if (categoryProperty != null && categoryProperty.ToLower() == "q")
                {
                    break;
                }

                if (category == null)
                {
                    Utilities.PrintMessage("Category not found. Please try again", false, true);
                    categoryProperty = Console.ReadLine();
                }
            }

            if(category == null)
            {
                category = categoryList[0];
            }

            return category;
        }

        private Category FindCategory()
        {
            Category? category = null;

            while (category == null)
            {
                string categoryName = Utilities.GetUserInput("category name. If not known, enter n to skip or a to exit to app menu").ToLower();
                if (categoryName == "a")
                {
                    break;
                }
                category = selectedAccount.CategoryList.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
                if (categoryName == "n")
                {
                    int categoryId = Validator.Convert<int>("category id");
                    category = selectedAccount.CategoryList.FirstOrDefault(c => c.Id == categoryId);
                }

                if (category == null)
                {
                    Utilities.PrintMessage("Sorry, category not found. Please try again", false, false);
                }
            }

            if (category == null)
            {
                throw new NullReferenceException();

            }
            return category;
        }

        private void UpdateCategoryDetails()
        {
            var category = FindCategory();

            AppScreen.DisplayCategoryUpdateDetails();

            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    UpdateCategoryName(category);
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    UpdateCategoryDetails();
                    break;
            }
        }

        private void UpdateCategoryName(Category category)
        {
            var name = Utilities.GetUserInput("new name");
            category.Name = name;
        }
    }
}

