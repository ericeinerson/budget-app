using System;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public partial class BudgetApp
	{
        private decimal _sumOfAllWishlistExpenses;

        public void ProcessWishlistMenuOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case (int)WishlistOption.ViewWishlist:
                    Console.WriteLine("View Wishlist");
                    ViewWishlist();
                    Utilities.PressEnterToContinue();
                    break;
                case (int)WishlistOption.AddWishlistItem:
                    Console.WriteLine("Add Wishlist Item");
                    AddWishlistItem();
                    Utilities.PressEnterToContinue();
                    break;
                case (int)WishlistOption.PayForWishlistItem:
                    Console.WriteLine("Pay For Wishlist Item");
                    PayForWishlistItem();
                    break;
                case (int)WishlistOption.UpdateItemDetails:
                    UpdateItemDetails();
                    break;
                case (int)WishlistOption.Logout:
                    AppScreen.LogoutProgress();
                    Run();
                    break;
                case (int)WishlistOption.GoBack:
                    GoBackToAppScreen();
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    ProcessWishlistOption();
                    break;
            }
        }

        private void ViewWishlist()
        {
            foreach (WishlistItem item in selectedAccount.Wishlist.Items)
            {
                Console.WriteLine($"Item: {item.Item}, Cost: {Utilities.FormatAmount(item.Cost)}, Priority: {item.Priority}, Id: {item.Id}\n");
            }
        }

        private void AddWishlistItem()
        {
            selectedAccount.WishlistId++;
            string newItemName = Utilities.GetUserInput("item name");
            decimal newItemCost = Validator.Convert<decimal>("item cost");
            int newItemPriority = Validator.Convert<int>("item priority");
            foreach (WishlistItem item in selectedAccount.Wishlist.Items)
            {
                if (newItemPriority <= item.Priority)
                {
                    item.Priority++;
                    Utilities.PrintMessage($"{item.Item}'s new priority is {item.Priority}", true);
                }
            }
            selectedAccount.Wishlist.Items.Add(new WishlistItem { Item = newItemName, Cost = newItemCost, Priority = newItemPriority, Id = selectedAccount.WishlistId });
        }

        private void PayForWishlistItem()
        {
            foreach (WishlistItem item in selectedAccount.Wishlist.Items)
            {
                Console.WriteLine($"Item: {item.Item}, Cost: {Utilities.FormatAmount(item.Cost)}, Priority: {item.Priority}, Id: {item.Id}\n");
            }

            int wishListItemId = Validator.Convert<int>("wishlist id");

            try
            {
                WishlistItem? selectedItem = selectedAccount.Wishlist.Items.Find(item => item.Id == wishListItemId);

                if(selectedItem == null)
                {
                    throw new NullReferenceException();
                }

                _sumOfAllWishlistExpenses += selectedItem.Cost;
                foreach (WishlistItem item in selectedAccount.Wishlist.Items)
                {
                    if (item.Priority > selectedItem.Priority)
                    {
                        item.Priority--;
                    }
                }
                selectedAccount.Wishlist.Items.Remove(selectedItem);
                Utilities.PrintMessage($"Success. Your new wishlist balance is {Utilities.FormatAmount(_sumOfAllWishlistExpenses)}", true);
            }
            catch
            {
                Utilities.PrintMessage("Invalid input. Please try again.", false);
                ProcessWishlistOption();
            }
        }

        private WishlistItem FindItem()
        {
            WishlistItem? item = null;

            while (item == null)
            {
                string itemName = Utilities.GetUserInput("item name. If not known, enter n to skip or a to exit to app menu").ToLower();
                if (itemName == "a")
                {
                    break;
                }
                item = selectedAccount.Wishlist.Items.FirstOrDefault(i => i.Item.ToLower() == itemName.ToLower());
                if (itemName == "n")
                {
                    decimal itemAmount = Validator.Convert<decimal>("item amount");
                    item = selectedAccount.Wishlist.Items.FirstOrDefault(i => i.Cost == itemAmount);
                }

                if (item == null)
                {
                    Utilities.PrintMessage("Sorry, item not found. Please try again", false, false);
                }
            }

            if (item == null)
            {
                throw new NullReferenceException();

            }
            return item;
        }

        private void UpdateItemDetails()
        {
            var item = FindItem();

            AppScreen.DisplayItemUpdateDetails();

            switch (Validator.Convert<int>("an option"))
            {
                case 1:
                    UpdateItemName(item);
                    break;
                case 2:
                    UpdateItemCost(item);
                    break;
                case 3:
                    UpdateItemPriority(item);
                    break;
                case 4:
                    UpdateAll(item);
                    break;
                default:
                    Utilities.PrintMessage("Invalid Option. Try again", false);
                    UpdateItemDetails();
                    break;
            }
        }

        private void UpdateItemName(WishlistItem item)
        {
            var name = Utilities.GetUserInput("new name");
            item.Item = name;
        }

        private void UpdateItemCost(WishlistItem item)
        {
            var cost = Validator.Convert<decimal>("new cost");
            item.Cost = cost;
        }

        private void UpdateItemPriority(WishlistItem item)
        {
            var priority = Validator.Convert<int>("new priority");
            item.Priority = priority;
        }

        private void UpdateAll(WishlistItem item)
        {
            UpdateItemName(item);
            UpdateItemCost(item);
            UpdateItemPriority(item);
        }
    }
}

