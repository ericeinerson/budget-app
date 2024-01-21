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
            wishlistId++;
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
            selectedAccount.Wishlist.Items.Add(new WishlistItem { Item = newItemName, Cost = newItemCost, Priority = newItemPriority, Id = wishlistId });
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
    }
}

