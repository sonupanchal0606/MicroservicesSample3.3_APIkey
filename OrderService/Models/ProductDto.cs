namespace OrderService.Models
{
	public class ProductDto
	{
		public Guid Id { get; set; }  // Same as ProductId from ProductService
		public string Name { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public decimal Price { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}	
}
