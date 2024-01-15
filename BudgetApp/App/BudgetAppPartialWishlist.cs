using System;
using BudgetApp.Domain.Entities;
using BudgetApp.UI;

namespace BudgetApp.App
{
	public partial class BudgetApp
	{
        //public void ManageWishList()
        //{
        //    AppScreen.DisplayWishlistOptions();
        //    ProcessWishlistOption();
        //}

        //public void ProcessWishlistOption()
        //{
        //    switch (Validator.Convert<int>("an option"))
        //    {
        //        case 1:
        //            Console.WriteLine("View Wishlist");
        //            foreach (WishlistItem item in selectedAccount.Wishlist.Items)
        //            {
        //                Console.WriteLine($"Item: {item.Item}, Cost: {Utilities.FormatAmount(item.Cost)}, Priority: {item.Priority}, Id: {item.Id}\n");
        //            }
        //            Utilities.PressEnterToContinue();
        //            break;
        //        case 2:
        //            Console.WriteLine("Add Wishlist Item");
        //            _wishlistIdCounter++;
        //            string newItemName = Utilities.GetUserInput("item name");
        //            decimal newItemCost = Validator.Convert<decimal>("item cost");
        //            int newItemPriority = Validator.Convert<int>("item priority");
        //            foreach (WishlistItem item in selectedAccount.Wishlist.Items)
        //            {
        //                if (newItemPriority <= item.Priority)
        //                {
        //                    item.Priority++;
        //                    Utilities.PrintMessage($"{item.Item}'s new priority is {item.Priority}", true);
        //                }
        //            }
        //            selectedAccount.Wishlist.Items.Add(new WishlistItem { Item = newItemName, Cost = newItemCost, Priority = newItemPriority, Id = _wishlistIdCounter });

        //            break;
        //        case 3:
        //            Console.WriteLine("Pay For Wishlist Item");
        //            foreach (WishlistItem item in selectedAccount.Wishlist.Items)
        //            {
        //                Console.WriteLine($"Item: {item.Item}, Cost: {Utilities.FormatAmount(item.Cost)}, Priority: {item.Priority}, Id: {item.Id}\n");
        //            }

        //            int wishListItemId = Validator.Convert<int>("wishlist id");
        //            try
        //            {
        //                WishlistItem selectedItem = selectedAccount.Wishlist.Items.Find(item => item.Id == wishListItemId);
        //                _sumOfAllWishlistExpenses += selectedItem.Cost;
        //                foreach (WishlistItem item in selectedAccount.Wishlist.Items)
        //                {
        //                    if (item.Priority > selectedItem.Priority)
        //                    {
        //                        item.Priority--;
        //                    }
        //                }
        //                selectedAccount.Wishlist.Items.Remove(selectedItem);
        //                Utilities.PrintMessage($"Success. Your new wishlist balance is {Utilities.FormatAmount(_sumOfAllWishlistExpenses)}", true);
        //            }
        //            catch
        //            {
        //                Utilities.PrintMessage("Invalid input. Please try again.", false);
        //                ProcessWishlistOption();
        //            }

        //            break;
        //        case 4:
        //            AppScreen.LogoutProgress();
        //            Utilities.PrintMessage("You have successfully logged out.", true);
        //            Run();
        //            break;
        //        default:
        //            Utilities.PrintMessage("Invalid Option. Try again", false);
        //            ProcessWishlistOption();
        //            break;
        //    }
        //}
    }
}

