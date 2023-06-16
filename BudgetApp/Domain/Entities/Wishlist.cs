using System;
namespace BudgetApp.Domain.Entities
{
	public class WishlistItem
	{
		public WishlistItem()
		{
			Item = "test item";
			Cost = 0;
			Id = 0;
			Priority = 0;
		}
		public string Item { get; set; }
		public decimal Cost { get; set; }
		public int Id { get; set; }
		public int Priority { get; set; }
	}

	public class Wishlist
	{
		public Wishlist()
		{
			Items = new List<WishlistItem>();
		}

		public List<WishlistItem> Items { get; set; }

	}
}

