namespace OrderService.Models
{
	public class OrderResponseDto
	{
		public Guid Id { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }

		public ProductDto Product { get; set; }
	}
}
