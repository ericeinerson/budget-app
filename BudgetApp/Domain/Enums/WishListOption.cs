using System;
using System.ComponentModel;

namespace BudgetApp.Domain.Enums
{
	public enum WishlistOption
	{
        [Description("View Wishlist")]
        ViewWishlist = 1,
        [Description("Add Wishlist Item")]
        AddWishlistItem,
        [Description("Pay For Wishlist Item")]
        PayForWishlistItem,
        [Description("Logout")]
        Logout,
        [Description("Go Back")]
        GoBack
	}
}

