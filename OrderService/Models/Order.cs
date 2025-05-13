namespace OrderService.Models
{
	public class Order
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public int Quantity { get; set; }

		// public Product Product { get; set; } // Navigation property. No Product navigation property here — no EF foreign key!
		public decimal TotalPrice { get; set; }
	}
}
